using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.BuilderIO.Core;
using VirtoCommerce.BuilderIO.Core.Services;
using VirtoCommerce.Pages.Core.ContentProviders;
using VirtoCommerce.Pages.Core.Models;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.BuilderIO.Data.ContentProviders;

public class BuilderIoContentProvider(
    IBuilderIoApiClient apiClient,
    IStoreService storeService,
    ISettingsManager settingsManager)
    : IPageContentProvider
{
    private const int PageSize = 100;

    public string ProviderName => "Builder.io";
    public bool SupportsReindexation => true;

    public async Task<long> GetTotalChangesCountAsync(DateTime? startDate, DateTime? endDate)
    {
        long totalCount = 0;

        await ForEachStoreAsync(async apiKey =>
        {
            var response = await apiClient.GetContentAsync(apiKey, ModuleConstants.PageModelName, limit: 0, offset: 0, updatedAfter: startDate);
            totalCount += response.TotalCount;
        });

        return totalCount;
    }

    public async Task<IList<IndexDocumentChange>> GetChangesAsync(DateTime? startDate, DateTime? endDate, long skip, long take)
    {
        var allChanges = new List<IndexDocumentChange>();

        await ForEachStoreAsync(async apiKey =>
        {
            var offset = 0;
            while (true)
            {
                var response = await apiClient.GetContentAsync(apiKey, ModuleConstants.PageModelName, limit: PageSize, offset: offset, updatedAfter: startDate);

                foreach (var page in response.Results)
                {
                    allChanges.Add(new IndexDocumentChange
                    {
                        DocumentId = page.Id,
                        ChangeDate = page.LastUpdated ?? page.CreatedDate,
                        ChangeType = IndexDocumentChangeType.Modified,
                    });
                }

                offset += PageSize;
                if (offset >= response.TotalCount || response.Results.Count == 0)
                {
                    break;
                }
            }
        });

        return allChanges
            .OrderByDescending(x => x.ChangeDate)
            .Skip(Convert.ToInt32(skip))
            .Take(Convert.ToInt32(take))
            .ToList();
    }

    public async Task<IList<PageDocument>> GetByIdsAsync(IList<string> ids)
    {
        var result = new List<PageDocument>();

        await ForEachStoreAsync(async (apiKey, storeId) =>
        {
            var response = await apiClient.GetContentByIdsAsync(apiKey, ModuleConstants.PageModelName, ids);

            foreach (var page in response.Results)
            {
                var pageDocument = page.ToPageDocument();
                pageDocument.StoreId = storeId;
                result.Add(pageDocument);
            }
        });

        return result;
    }

    private async Task ForEachStoreAsync(Func<string, Task> action)
    {
        await ForEachStoreAsync(async (apiKey, _) => await action(apiKey));
    }

    private async Task ForEachStoreAsync(Func<string, string, Task> action)
    {
        var stores = await storeService.GetAsync([], null, clone: false);
        if (stores == null)
        {
            return;
        }

        foreach (var store in stores)
        {
            var enabled = await settingsManager.GetValueAsync<bool>(ModuleConstants.Settings.General.Enable);
            if (!enabled)
            {
                continue;
            }

            var apiKey = await settingsManager.GetValueAsync<string>(ModuleConstants.Settings.General.PublicApiKey);
            if (string.IsNullOrEmpty(apiKey))
            {
                continue;
            }

            await action(apiKey, store.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.BuilderIO.Core.Models;

namespace VirtoCommerce.BuilderIO.Core.Services;

public interface IBuilderIoApiClient
{
    Task<BuilderIoContentResponse> GetContentAsync(string apiKey, string modelName, int limit, int offset, DateTime? updatedAfter = null, DateTime? updatedBefore = null, bool includeUnpublished = false);
    Task<BuilderIoContentResponse> GetContentByIdsAsync(string apiKey, string modelName, IList<string> ids);
}

public class BuilderIoContentResponse
{
    public IList<BuilderIOPage> Results { get; set; } = [];
    public int TotalCount { get; set; }
}

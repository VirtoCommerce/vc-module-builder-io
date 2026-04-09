using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.BuilderIO.Core.Models;
using VirtoCommerce.BuilderIO.Core.Services;

namespace VirtoCommerce.BuilderIO.Data.Services;

public class BuilderIoApiClient(IHttpClientFactory httpClientFactory) : IBuilderIoApiClient
{
    private const string BaseUrl = "https://cdn.builder.io/api/v3/content";
    private const int MaxLimit = 100;

    public async Task<BuilderIoContentResponse> GetContentAsync(
        string apiKey,
        string modelName,
        int limit,
        int offset,
        DateTime? updatedAfter = null,
        bool includeUnpublished = false)
    {
        var queryParams = new List<string>
        {
            $"apiKey={Uri.EscapeDataString(apiKey)}",
            $"limit={Math.Min(limit, MaxLimit)}",
            $"offset={offset}",
            "totalCount=true",
            "noTargeting=true",
            "sort.lastUpdated=-1",
        };

        if (updatedAfter.HasValue)
        {
            var unixMs = new DateTimeOffset(updatedAfter.Value).ToUnixTimeMilliseconds();
            queryParams.Add($"query.lastUpdated.$gte={unixMs}");
        }

        if (includeUnpublished)
        {
            queryParams.Add("includeUnpublished=true");
        }

        var url = $"{BaseUrl}/{Uri.EscapeDataString(modelName)}?{string.Join("&", queryParams)}";
        return await FetchAsync(url);
    }

    public async Task<BuilderIoContentResponse> GetContentByIdsAsync(
        string apiKey,
        string modelName,
        IList<string> ids)
    {
        // Builder.io supports querying by ID using query.id.$in
        var response = new BuilderIoContentResponse();

        // Process in chunks of MaxLimit
        foreach (var chunk in ids.Chunk(MaxLimit))
        {
            var idsJson = JsonConvert.SerializeObject(chunk);
            var queryParams = new List<string>
            {
                $"apiKey={Uri.EscapeDataString(apiKey)}",
                $"limit={chunk.Length}",
                "noTargeting=true",
                $"query.id.$in={Uri.EscapeDataString(idsJson)}",
            };

            var url = $"{BaseUrl}/{Uri.EscapeDataString(modelName)}?{string.Join("&", queryParams)}";
            var chunkResponse = await FetchAsync(url);
            response.Results = response.Results.Concat(chunkResponse.Results).ToList();
            response.TotalCount += chunkResponse.TotalCount;
        }

        return response;
    }

    private async Task<BuilderIoContentResponse> FetchAsync(string url)
    {
        var client = httpClientFactory.CreateClient("BuilderIo");
        var httpResponse = await client.GetAsync(url);
        httpResponse.EnsureSuccessStatusCode();

        var json = await httpResponse.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(json);

        var results = jObject["results"]?.ToObject<List<BuilderIOPage>>() ?? [];
        var totalCount = jObject["totalCount"]?.Value<int>() ?? results.Count;

        return new BuilderIoContentResponse
        {
            Results = results,
            TotalCount = totalCount,
        };
    }
}

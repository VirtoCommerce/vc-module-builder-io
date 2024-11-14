using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.BuilderIO.Core.Converters;
using VirtoCommerce.Pages.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BuilderIO.Core.Models;

public class BuilderIOPage
{
    [JsonProperty("@version")]
    public int Version { get; set; }
    public string CreatedBy { get; set; }

    [JsonConverter(typeof(UnixMillisecondsJsonConverter))]
    public DateTime CreatedDate { get; set; }
    [JsonConverter(typeof(UnixMillisecondsJsonConverter))]
    public DateTime StartDate { get; set; }
    [JsonConverter(typeof(UnixMillisecondsJsonConverter))]
    public DateTime EndDate { get; set; }
    public Dictionary<string, object> Data { get; set; }

    [JsonConverter(typeof(UnixMillisecondsJsonConverter))]
    public DateTime? FirstPublished { get; set; }
    public string Id { get; set; }
    public string LastUpdateBy { get; set; }

    [JsonConverter(typeof(UnixMillisecondsJsonConverter))]
    public DateTime? LastUpdated { get; set; }
    public string LastUpdatedBy { get; set; }
    public PageMetadata Meta { get; set; }
    public string ModelId { get; set; }
    public string Name { get; set; }
    public string OwnerId { get; set; }
    public float Priority { get; set; }
    public string Published { get; set; }
    public PageQuery[] Query { get; set; }

    public virtual PageDocument ToPageDocument()
    {
        var pageDocument = AbstractTypeFactory<PageDocument>.TryCreateInstance();
        pageDocument.Content = GetDataProperty("blocksString");
        pageDocument.CreatedBy = CreatedBy;
        pageDocument.CreatedDate = CreatedDate;
        pageDocument.Id = Id;
        pageDocument.OuterId = Id;
        pageDocument.Permalink = GetQueryProperty("urlPath");
        pageDocument.UserGroups = GetQueryProperty("groupName")
            ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();
        pageDocument.Title = GetDataProperty("title");
        pageDocument.Description = GetDataProperty("description");
        pageDocument.MimeType = "application/json";
        pageDocument.ModifiedBy = LastUpdatedBy;
        pageDocument.ModifiedDate = LastUpdated;
        pageDocument.Source = "builder.io";
        pageDocument.Visibility = GetQueryProperty("visibility") == "private"
            ? PageDocumentVisibility.Private
            : PageDocumentVisibility.Public;
        pageDocument.StartDate = StartDate;
        pageDocument.EndDate = EndDate == DateTime.MinValue ? DateTime.MaxValue : EndDate;
        return pageDocument;
    }

    private string GetDataProperty(string propertyName)
    {
        return Data?.GetValueOrDefault(propertyName)?.ToString();
    }

    private string GetQueryProperty(string propertyName, string operatorName = "is")
    {
        return Query
            ?.FirstOrDefault(x => x.Property == propertyName && x.Operator == operatorName)?.Value;
    }
}


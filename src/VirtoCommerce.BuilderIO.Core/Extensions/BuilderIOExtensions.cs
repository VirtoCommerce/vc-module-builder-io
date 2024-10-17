using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.BuilderIO.Core.Models;
using VirtoCommerce.Pages.Core.Events;
using VirtoCommerce.Pages.Core.Models;

namespace VirtoCommerce.BuilderIO.Core.Extensions;

public static class BuilderIOExtensions
{
    public static PageOperation ToPageOperation(this string value)
    {
        return value switch
        {
            "publish" => PageOperation.Publish,
            "archive" => PageOperation.Archive,
            "delete" => PageOperation.Delete,
            "unpublish" => PageOperation.Unpublish,
            "scheduledStart" => PageOperation.ScheduledStart,
            "scheduledEnd" => PageOperation.ScheduledEnd,
            _ => PageOperation.Unknown,
        };
    }

    public static PageDocument FromBuilderIOPage(this BuilderIOPage value, string storeId, string cultureName,
        PageOperation operation = PageOperation.Unknown)
    {
        return new PageDocument
        {
            Content = value.GetDataProperty("blocksString"),
            CreatedBy = value.CreatedBy,
            CreatedDate = value.CreatedDate,
            Id = value.Id,
            OuterId = value.Id,
            Permalink = value.GetQueryProperty("urlPath"),
            Title = value.Data?.GetValueOrDefault("title")?.ToString(),
            Description = value.GetDataProperty("description").ToString(),
            Status = operation.GetPageDocumentStatus(),
            MimeType = "application/json",
            ModifiedBy = value.LastUpdatedBy,
            ModifiedDate = value.LastUpdated,
            Source = "builder.io",
            Visibility = PageDocumentVisibility.Public,
            StoreId = storeId,
            CultureName = cultureName,
            StartDate = value.StartDate,
            EndDate = value.EndDate == DateTime.MinValue ? DateTime.MaxValue : value.EndDate,
            // UserGroups = 
        };
    }

    private static string GetDataProperty(this BuilderIOPage page, string propertyName)
    {
        return page.Data?.GetValueOrDefault(propertyName)?.ToString();
    }

    private static string GetQueryProperty(this BuilderIOPage page, string propertyName, string operatorName = "is")
    {
        return page.Query
            ?.FirstOrDefault(x => x.Property == propertyName && x.Operator == operatorName)?.Value;
    }

    public static PageDocumentStatus GetPageDocumentStatus(this PageOperation operation)
    {
        return operation switch
        {
            PageOperation.Publish => PageDocumentStatus.Published,
            PageOperation.Archive => PageDocumentStatus.Archived,
            PageOperation.Delete => PageDocumentStatus.Deleted,
            PageOperation.Unpublish => PageDocumentStatus.Draft,
            PageOperation.ScheduledStart => PageDocumentStatus.Published,
            PageOperation.ScheduledEnd => PageDocumentStatus.Archived,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}


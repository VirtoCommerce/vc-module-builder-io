using VirtoCommerce.Pages.Core.Events;

namespace VirtoCommerce.BuilderIO.Data.Extensions;

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
            _ => PageOperation.Unknown
        };
    }
}


using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BuilderIO.Core;
using VirtoCommerce.BuilderIO.Core.Models;
using VirtoCommerce.BuilderIO.Data.Extensions;
using VirtoCommerce.Pages.Core.Events;
using VirtoCommerce.Pages.Core.Extensions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.BuilderIO.Web.Controllers.Api;

[Authorize]
[Route("api/pages/builder-io")]
public class PagesBuilderIOController(IEventPublisher eventPublisher) : Controller
{
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] BuilderIOPageChanges model, [FromHeader] string storeId, [FromHeader] string cultureName)
    {
        // todo: permissions
        if (model?.ModelName == "page")
        {
            var pageOperation = model.Operation.ToPageOperation();
            if ((pageOperation == PageOperation.Delete &&
                 !User.HasGlobalPermission(ModuleConstants.Security.Permissions.Delete))
                || !User.HasGlobalPermission(ModuleConstants.Security.Permissions.Update))
            {
                return Forbid();
            }

            var pageDocument = (model.NewValue ?? model.PreviousValue).ToPageDocument();
            pageDocument.Status = pageOperation.GetPageDocumentStatus();
            pageDocument.StoreId = storeId;
            pageDocument.CultureName = cultureName;

            var pageChangedEvent = AbstractTypeFactory<PagesDomainEvent>.TryCreateInstance();
            pageChangedEvent.Operation = pageOperation;
            pageChangedEvent.Page = pageDocument;

            await eventPublisher.Publish(pageChangedEvent);
        }

        return Ok();
    }
}


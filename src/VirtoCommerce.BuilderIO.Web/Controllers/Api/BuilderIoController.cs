using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.BuilderIO.Core;

namespace VirtoCommerce.BuilderIO.Web.Controllers.Api;

[Route("api/builder-io")]
public class BuilderIOController : Controller
{
    const string BuilderIOSiteRedirectUrl = "https://builder.io/content";

    [HttpGet]
    [Route("redirect")]
    [Authorize(ModuleConstants.Security.Permissions.Access)]
    public ActionResult Redirect()
    {
        return Redirect(BuilderIOSiteRedirectUrl);
    }

}

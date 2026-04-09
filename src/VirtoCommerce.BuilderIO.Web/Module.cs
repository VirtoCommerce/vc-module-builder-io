using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.BuilderIO.Core;
using VirtoCommerce.BuilderIO.Core.Services;
using VirtoCommerce.BuilderIO.Data.ContentProviders;
using VirtoCommerce.BuilderIO.Data.Services;
using VirtoCommerce.Pages.Core.ContentProviders;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;

namespace VirtoCommerce.BuilderIO.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient("BuilderIo");
        serviceCollection.AddTransient<IBuilderIoApiClient, BuilderIoApiClient>();
        serviceCollection.AddTransient<BuilderIoContentProvider>();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        //Register store level settings
        settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreLevelSettings, nameof(Store));

        // Register permissions
        var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "BuilderIO", ModuleConstants.Security.Permissions.AllPermissions);

        // Register content provider for Pages module
        var contentProviderRegistrar = serviceProvider.GetService<IPageContentProviderRegistrar>();
        contentProviderRegistrar?.RegisterProvider(() => serviceProvider.GetRequiredService<BuilderIoContentProvider>());
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}

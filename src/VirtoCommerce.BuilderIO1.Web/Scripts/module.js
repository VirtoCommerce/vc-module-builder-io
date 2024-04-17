//Call this to register our module to main application
var moduleName = "virtoCommerce.builderioModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.widgetService',
        function (widgetService) {
        }]);

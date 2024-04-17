angular.module('BuilderIo')
    .controller('BuilderIo.helloWorldController', ['$scope', 'BuilderIo.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'BuilderIo';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'BuilderIo.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);

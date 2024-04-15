angular.module('BuilderIo')
    .factory('BuilderIo.webApi', ['$resource', function ($resource) {
        return $resource('api/builder-io');
    }]);

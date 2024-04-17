angular.module('BuilderIO')
    .factory('BuilderIO.webApi', ['$resource', function ($resource) {
        return $resource('api/builderio');
    }]);

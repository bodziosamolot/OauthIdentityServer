(function () {
    'use strict';

    angular
    .module('core.module')
    .factory('ValuesResource', ValuesResource);

    ValuesResource.$inject = ['$resource'];

    function ValuesResource($resource) {
        return {
            ManagementValues: $resource('https://localhost:44396/api/values/ManagementValues'),
            SecretValues: $resource('https://localhost:44396/api/values/SecretValues'),
            PublicValues: $resource('https://localhost:44396/api/values/PublicValues')
        };
    }
})();

(function () {
    'use strict';

    angular
        .module('core.module')
        .service('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['localStorageService'];

    function authInterceptorService(localStorageService) {

        var vm = this;

        vm.request = request;

        function request(config) {

            config.headers = config.headers || {};

            var token = localStorage["access_token"];
            if (token && (!config.params || !config.params.ommitBearerToken)) {
                config.headers.Authorization = 'Bearer ' + token;
                console.log("Using the following token:");
                console.log(token);
            }

            return config;
        }
    }
})();
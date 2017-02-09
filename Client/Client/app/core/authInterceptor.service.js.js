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

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }
    }
})();
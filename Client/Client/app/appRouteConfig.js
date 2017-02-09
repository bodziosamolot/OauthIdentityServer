(function () {
    'use strict';

    angular
        .module('app')
        .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', routeConfig]);

    function routeConfig($stateProvider, $urlRouterProvider, $httpProvider) {

        //$httpProvider.interceptors.push('authInterceptorService');

        $urlRouterProvider.otherwise('login');

        $stateProvider
            .state('login',
            {
                url: '/login',
                templateUrl: '/app/core/login.html',
                controller: 'LoginController',
                controllerAs: 'vm'
            });

    }
}());
(function () {
    'use strict';

    angular
        .module('app')
        .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', routeConfig]);

    function routeConfig($stateProvider, $urlRouterProvider, $httpProvider) {

        $httpProvider.interceptors.push('authInterceptorService');
        //$httpProvider.interceptors.push(function (OidcManager) {
        //    return {
        //        'request': function (config) {

        //            // if it's a request to the API, we need to provide the
        //            // access token as bearer token.
        //            if (config.url.indexOf("https://localhost:44396") === 0) {
        //                config.headers.Authorization = 'Bearer ' + OidcManager.OidcTokenManager().access_token;
        //            }

        //            return config;
        //        }

        //    };
        //});

        $urlRouterProvider.otherwise('home');

        $stateProvider
            .state('login',
            {
                url: '/login',
                templateUrl: '/app/core/login.html',
                controller: 'LoginController',
                controllerAs: 'vm'
            }).state('home',
            {
                url: '/home',
                templateUrl: '/app/core/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            }).state('loggedIn',
            {
                url: '/loggedIn',
                templateUrl: '/app/core/loggedIn.html',
                controller: 'loggedInController',
                controllerAs: 'vm'
            });

    }
}());
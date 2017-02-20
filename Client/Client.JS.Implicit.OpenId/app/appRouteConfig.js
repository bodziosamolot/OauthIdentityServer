(function () {
    'use strict';

    angular
        .module('app')
        .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', routeConfig]);

    function routeConfig($stateProvider, $urlRouterProvider, $httpProvider) {

        $httpProvider.interceptors.push('authInterceptorService');

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
(function () {
    'use strict';

    angular
        .module('app',
    [
        /*
         * Angular modules
         */
        'ngResource',
        'ngMessages',
        'ui.router',
        'ngResource',

        'core.module',

        'ui.bootstrap',
        'LocalStorageModule',
        'oauth2'

    ]);
}());
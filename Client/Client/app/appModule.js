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

        'core.module',

        'ui.bootstrap',
        'LocalStorageModule'

    ]);
}());
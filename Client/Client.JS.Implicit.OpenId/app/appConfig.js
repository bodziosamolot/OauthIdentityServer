(function () {
    'use strict';

    angular
        .module('app')
        .config(appConfig);

    function appConfig($provide) {
        $provide.decorator('$exceptionHandler', exceptionDelegate);
    }

    function exceptionDelegate() {
        return function (exception, cause) {
            alert(exception.message);
        };
    }
}());
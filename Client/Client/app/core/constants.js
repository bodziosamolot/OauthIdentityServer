(function () {
    'use strict';

    angular.module('core.module')
    .constant("config", {
        apiUrl: "http://localhost:60136",
        loginUrl: "http://localhost:60060/connect/authorize",
        issuerUri: "http://localhost:60060/",
        redirectUrl: "http://localhost:61488/#!/home"
    });
}());


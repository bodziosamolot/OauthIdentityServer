(function () {
    'use strict';

    angular.module('core.module')
    .constant("config", {
        apiUrl: "http://localhost:60682",
        loginUrl: "https://localhost:44308/identity/connect/authorize",
        issuerUri: "https://localhost:44308/identity",
        redirectUrl: "http://localhost:61488/#!/home"
    });
}());


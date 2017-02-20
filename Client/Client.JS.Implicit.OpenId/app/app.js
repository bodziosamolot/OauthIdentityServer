(function () {
    'use strict';

    angular
        .module('app')
        .run(app);

    app.$inject = ['oauthService', '$http', 'authService', 'config'];

    function app(oauthService, $http, authService, config) {
        oauthService.loginUrl = config.loginUrl;
        oauthService.redirectUri = config.redirectUrl;
        oauthService.clientId = "js";
        oauthService.scope = "api";
        oauthService.issuer = config.issuerUri;
        oauthService.oidc = true;

        oauthService.setup({
            loginState: 'login',
            onTokenReceived: function (context) {
                console.log('Token received!!!!!!!!!!!!!!!!');
                $http.defaults.headers.common['Authorization'] = 'Bearer ' + context.accessToken;
                authService.authentication.userName = context.idClaims['given_name'];
            }
        });
    };

}());
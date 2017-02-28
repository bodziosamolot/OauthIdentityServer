(function () {
    'use strict';

    angular
        .module('app')
        .run(app);

    app.$inject = ['oauthService', '$http', 'authService', 'config'];

    function app(oauthService, $http, authService, config) {
        //var config = {
        //    client_id: "angular_client_implicit_open_id",
        //    callbackURL: "https://localhost:44391/callback.html",
        //    post_logout_redirect_uri: "https://localhost:44391/home",
        //    response_type: "id_token token",
        //    scope: "openid profile management secret",
        //    authority: "https://localhost:44308/identity",
        //    verbose_logging: true
        //};

        //oauthService.loginUrl = "https://localhost:44308/identity/connect/authorize";
        //oauthService.redirectUri = "https://localhost:44391/callback.html";
        //oauthService.clientId = "angular_client_implicit_open_id";
        //oauthService.scope = "openid profile management secret";
        //oauthService.issuer = "https://localhost:44308/identity";
        ////oauthService.oidc = true;
        //oauthService.userStore = new Oidc.WebStorageStateStore({ store: localStorage });
        //oauthService.stateStore = new Oidc.WebStorageStateStore({ store: localStorage });

        //oauthService.setup({
        //    loginState: 'login',
        //    onTokenReceived: function (context) {
        //        $http.defaults.headers.common['Authorization'] = 'Bearer ' + context.accessToken;
        //        userService.userName = context.idClaims['given_name'];
        //    }
        //});

    };

}());
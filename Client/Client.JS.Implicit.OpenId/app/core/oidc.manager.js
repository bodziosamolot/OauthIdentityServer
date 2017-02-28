(function () {
    'use strict';

// oidc manager for dep injection
    angular.module("core.module")
        .factory("OidcManager", function () {

            var settings = {
                authority: 'https://localhost:44308/identity',
                client_id: 'angular_client_implicit_open_id',
                redirect_uri: 'https://localhost:44391/callback.html',
                post_logout_redirect_uri: 'https://localhost:44391/logout_callback.html',
                EnablePostSignOutAutoRedirect: true,
                response_type: 'id_token token',
                scope: 'openid profile management secret address',
                filterProtocolClaims: true,
                loadUserInfo: true,
                userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),

                silent_redirect_uri: window.location.protocol + "//" + window.location.host + '/silent_callback.html',
                automaticSilentRenew: true
            };

            var mgr = new Oidc.UserManager(settings);

            mgr.events.addAccessTokenExpired(function () {
                console.log("token expired");
            });

            mgr.events.addSilentRenewError(function (e) {
                console.log("silent renew error", e.message);
            });

            mgr.events.addUserLoaded(function (user) {
                console.log("user loaded", user);
                mgr.getUser().then(function () {
                    console.log("getUser loaded user after userLoaded event fired");
                });
            });

            return {
                OidcTokenManager: function () {
                    return mgr;
                }
            };
        });
})();
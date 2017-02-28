(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['authService', '$scope', 'oauthService', 'SecurityService', 'ValuesResource', 'OidcManager'];

    function HomeController(authService, $scope, oauthService, SecurityService, ValuesResource, OidcManager) {

        var vm = this;

        Oidc.Log.logger = console;
        Oidc.Log.level = Oidc.Log.INFO;

        //vm.mgr = OidcManager.OidcTokenManager();
        vm.getData = _getData;
        vm.logOut = _logOut;
        vm.performLogin = _performLogin;
        vm.iFrameLogin = _iFrameLogin;
        vm.publicValues = undefined;
        vm.clearLocalStorage = _clearLocalStorage;
        vm.managementValues = undefined;
        vm.secretValues = undefined;
        vm.getUser = _getUser;
        vm.userName = undefined;
        vm.managementAuthorized = true;
        vm.secretAuthorized = true;

        var mgr = OidcManager.OidcTokenManager();

        function _clearLocalStorage() {
            localStorage.clear();
        }

        function _getUser() {
            mgr.getUser().then(function (user) {
                console.log("got user"+ user.profile.given_name);
            }).catch(function (err) {
                console.log(err);
            });
        }

        function _performLogin() {
            mgr.signinRedirect().then(function () {
            }).catch(function (err) {
            });
        }

        function _iFrameLogin() {
            mgr.signinSilent().then(function () {
            }).catch(function (err) {
            });
        }

        function _logOut() {

            mgr.signoutRedirect().then(function () {
                console.log('logged out');
            }).catch(function (err) {
            });
        }

        function _getData() {
            ValuesResource.PublicValues.query({ommitBearerToken: true}, function (response) {
                vm.publicValues = response;
            }, function (error) {
                console.log(error);
            });

            ValuesResource.ManagementValues.query({}, function (response) {
                vm.managementValues = response;
                vm.managementAuthorized = true;
            },function(error) {
                console.log(error);
                vm.managementAuthorized = false;
            });

            ValuesResource.SecretValues.query({}, function (response) {
                vm.secretValues = response;
                vm.secretAuthorized = true;
            }, function (error) {
                console.log(error);
                vm.secretAuthorized = false;
            });
        }

    }

}());
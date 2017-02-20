(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['authService', '$scope', 'oauthService', 'SecurityService', 'ValuesResource'];

    function HomeController(authService, $scope, oauthService, SecurityService, ValuesResource) {

        var vm = this;

        vm.initImplicit = _initImplicit;
        vm.performLogin = _performLogin;
        vm.getData = _getData;
        vm.clearStorage = _clearStorage;
        vm.publicValues = undefined;
        vm.managementValues = undefined;
        vm.secretValues = undefined;
        vm.authorized = true;

        getLoginUrl();

        function getLoginUrl() {
            oauthService.createLoginUrl().then(function (url) {
                console.log(url);
                vm.loginUrl = url;
            });
        }

        function _initImplicit() {
            oauthService.initImplicitFlow('login');
        }

        function _performLogin() {
            SecurityService.DoAuthorization();
        }

        function _clearStorage() {
            localStorage['access_token'] = undefined;
        }

        function _getData() {
            ValuesResource.PublicValues.query({ommitBearerToken: true}, function (response) {
                vm.publicValues = response;
            }, function (error) {
                console.log(error);
            });

            ValuesResource.ManagementValues.query({}, function (response) {
                vm.managementValues = response;
                vm.authorized = true;
            },function(error) {
                console.log(error);
                vm.authorized = false;
            });

            ValuesResource.SecretValues.query({}, function (response) {
                vm.secretValues = response;
            }, function (error) {
                console.log(error);
            });
        }

    }

}());
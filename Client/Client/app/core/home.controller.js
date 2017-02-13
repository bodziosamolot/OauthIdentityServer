(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['authService', '$scope', 'loginService', 'oauthService', 'SecurityService'];

    function HomeController(authService, $scope, loginService, oauthService, SecurityService) {

        var vm = this;

        vm.initImplicit = _initImplicit;
        vm.performLogin = _performLogin;

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

    }

}());
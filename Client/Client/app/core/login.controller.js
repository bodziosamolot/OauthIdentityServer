(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$state', '$scope', 'authService'];

    function LoginController($state, $scope, authService) {

        var vm = this;

        vm.user = {};
        vm.errorMessage = undefined;
        vm.performLogin = performLogin;

        activate();

        function activate() {
            if (authService.isLoggedIn()) {
                $state.go('home');
            }
        }

        function performLogin(userName, password) {
            authService.login({
                username: userName,
                password: password,
                grant_type: "password"
            }).then(function () {
                if (authService.authentication.isAuth) {
                    $state.go('home');
                } else {
                    $scope.errorMessage = "Not logged in";
                }
            }, function (error) {
                console.log(error);
            });

        }
    }

}());
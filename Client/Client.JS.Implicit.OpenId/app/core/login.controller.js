(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$state', '$scope', 'oauthService', 'authService', 'loginService'];

    function LoginController($state, $scope, oauthService, authService, loginService) {

        var vm = this;
        vm.login = _login;

        function _login(userName, password) {
            authService.login({ userName: userName, password: password }).then(function(response) {
                console.log(response);
            }, function(error) {
                console.log('error ');
                console.log(error);
            });
            //loginService.loginUser().then(function (response){console.log(response)}).error(function (response){console.log(response)});
        }


    }

}());
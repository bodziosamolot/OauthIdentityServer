(function () {
    'use strict';

    angular
        .module('core.module')
        .service('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService'];

    function authService($http, $q, localStorageService) {

        var vm = this;

        vm.authentication = {};
        vm.saveRegistration = saveRegistration;
        vm.login = login;
        vm.logOut = logOut;
        vm.isLoggedIn = isLoggedIn;

        activate();

        function activate() {
            var localData = localStorageService.get('authorizationData');
            if (localData) {
                vm.authentication = localData;
            } else {
                vm.authentication = {
                    isAuth: false,
                    userName: "",
                    roles: []
                };
            }
        }

        function saveRegistration(registration) {

            //as.logOut();

            return $http.post(
                constants.serverPath + 'api/accounts',
                registration).then(function (response) {
                    return response;
                });

        };

        function login(loginData) {

            var data = "grant_type=password&username=" + loginData.username +
                "&password=" + loginData.password;

            var deferred = $q.defer();

            $http.post(
                constants.tokenServerPath + "oauth/token",
                data,
                {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).success(function (response) {

                    localStorageService.set(
                        'authorizationData',
                        {
                            token: response.access_token,
                            userName: loginData.username,
                            roles: response.roles.split(','),
                            isAuth: true
                        });

                    localStorageService.get('authorizationData');

                    vm.authentication.isAuth = true;
                    vm.authentication.userName = loginData.username;
                    vm.authentication.roles = response.roles.split(',');

                    deferred.resolve(response);

                }).error(function (err, status) {
                    vm.logOut();
                    deferred.reject(err);
                    console.log("error: " + err);
                });

            return deferred.promise;

        };

        function logOut() {

            localStorageService.remove('authorizationData');

            vm.authentication.isAuth = false;
            vm.authentication.userName = "";
            vm.authentication.roles = undefined;

        };

        function isLoggedIn() {

            if (!vm.authentication) {
                vm.authentication = localStorageService.get('authorizationData');
            }

            return vm.authentication && vm.authentication.isAuth && vm.authentication.userName;
        }
    }
})();
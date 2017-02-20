(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('LoggedInController', LoggedInController);

    LoggedInController.$inject = ['authService', '$scope', 'loginService', 'oauthService', 'SecurityService'];

    function LoggedInController() {

        var vm = this;
    }

}());
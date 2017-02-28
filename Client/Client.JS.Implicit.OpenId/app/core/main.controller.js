(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('MainController', MainController);

    MainController.$inject = ['authService'];

    function MainController(authService) {

        //var vm = this;
        //vm.mgr = OidcManager.OidcTokenManager();
        //if (vm.mgr.expired) {
        //    vm.mgr.redirectForToken();
        //}

    }
})();

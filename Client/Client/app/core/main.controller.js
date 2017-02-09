(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('MainController', MainController);

    MainController.$inject = ['authService'];

    function MainController(authService) {

        var vm = this;

    }
})();

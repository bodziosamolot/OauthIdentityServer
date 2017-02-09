(function () {
    'use strict';

    angular
        .module('core.module')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['authService', '$scope'];

    function HomeController(menuService, authService, $scope) {

        var vm = this;
        vm.test = "Test ss";

    }

}());
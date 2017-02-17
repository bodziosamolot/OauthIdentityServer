(function () {
    'use strict';

    angular
        .module('core.module')
        .directive('restrict', restrictDirective);

    restrictDirective.$inject = ['authService'];

    function restrictDirective(authService) {
        return {
            restrict: 'A',
            priority: 100000,
            scope: false,
            compile: function (element, attr, linker) {

            },
            link: function (scope) {
                scope.$watch(authService.authentication.isAuth, function (newValue, oldValue) {
                    console.log('restrict changed');
                }, true);
            }
        }
    }
}());
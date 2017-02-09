(function () {
    'use strict';

    angular
        .module('core.module')
        .service('loginService', loginService);

    loginService.$inject = ['$resource', 'constants'];

    function loginService($resource, constants) {
        return $resource(constants.tokenServerPath + "/token", {},{
            loginUser: {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (data, headersGetter) {
                    var str = [];
                    for (var d in data)
                        str.push(encodeURIComponent(d) + "=" + encodeURIComponent(data[d]));
                    return str.join("&");
                }
            }
        });
    }
})();

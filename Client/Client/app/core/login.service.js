(function () {
    'use strict';

    angular
        .module('core.module')
        .service('loginService', loginService);

    loginService.$inject = ['$resource', 'config'];

    function loginService($resource, config) {
        return $resource(config.issuerUri, {userName: 'bob', password: 'secret'},{
            loginUser: {
                method: 'GET',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                //transformRequest: function (data, headersGetter) {
                //    var str = [];
                //    for (var d in data)
                //        str.push(encodeURIComponent(d) + "=" + encodeURIComponent(data[d]));
                //    return str.join("&");
                //}
            }
        });
    }
})();

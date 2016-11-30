(function () {
"use strict";

    angular.module('ContactMgmtApp')
    .service('ContactMgmtSvc', MenuService);


    MenuService.$inject = ['$http'];

    function MenuService($http) {
        var service = this;

        service.getGroups = function () {
            return $http.get('http://localhost:21395/api/contactsapp/Groups/0/1000').then(function success(response) {
                debugger;
                return response.data;
            }, function error(response) {
                alert('something went wrong while fetching groups')
                console.log(response);
            });
        };
    };
  

})();

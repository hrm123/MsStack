( function(){
    var app = angular.module('ContactMgmtApp');
    app.controller('ContactMgmtController', menuController);


    menuController.$inject = ["$http","$scope","$stateParams"];
    
    function menuController($http, $scope, $stateParams) {
              
        $scope.mainGridOptions = {
            dataSource: {
                type: "json",
                transport: {
                        read:{
                            url : 'http://localhost:21395/api/contactsapp/contacts/',
                            dataType: 'json'

                        }
                    },
                    
                    /*
                    read: function (e) {
                        var requestData = {
                            page: !(e.data.page) ? 1 : e.data.page,
                            pageSize: (!e.data.pageSize) ? 10: e.data.pageSize
                        };
                        var url1 = 'http://localhost:21395/api/contactsapp/contacts/?page=' + requestData.page + "&pageSize=" + requestData.pageSize;
                        $http.get(url1)
                          .then(function success(response) {
                              debugger;
                              e.success(response.data.PayLoad);
                          }, function error(response) {
                              alert('something went wrong')
                              console.log(response);
                          })


                    },
                    */
                    pageSize: 10,
                    pageable: {
                        refresh: true,
                        pageSizes: [10, 100, 200],

                    },
                    serverPaging: true,
                    serverSorting: false,
                    schema: {
                     
                        data: function (response) {
                            return response.PayLoad;
                        },
                        total: function (response) {
                            return response.AddnlInfo; // total is returned in the "total" field of the response
                        }
                        
                    }
                },
            columns: [{
                field: "FirstName",
                title: "First Name",
                width: "120px"
            }, {
                field: "LastName",
                title: "Last Name",
                width: "120px"
            }],
            sortable: false,
            pageable: true
          
        }
    }
        
})();

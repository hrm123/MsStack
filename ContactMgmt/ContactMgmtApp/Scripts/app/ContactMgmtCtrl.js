( function(){
    var app = angular.module('ContactMgmtApp');
    app.controller('ContactMgmtController', menuController);


    menuController.$inject = ["$http","$scope","$stateParams","groupsList"];
    
    function menuController($http, $scope, $stateParams,groupsList) {
              
        debugger;
        $scope.groups = groupsList;
        $scope.selectedGrpIds = [];

        $scope.newContact = {
            FirstName: '',
            LastName: '',
            groups: [],
            'firsNameLen': 4,
            'lastNameLen': 4,
            'maxLenName': 20
        }



        $scope.selectGrpOptions = {
            placeholder: "Select groups...",
            dataTextField: "GroupName",
            dataValueField: "GroupId",
            valuePrimitive: true,
            autoBind: false,
            dataSource: $scope.groups
        };
        

        $scope.onSelectCallback = function (someParam, selectedItems) {
            debugger;
            /*
            console.log("on select calback");
            console.log(someParam);
            console.log(selectedItems);
            console.log("on select calback");
            */
        };

/*        
        //function getGroups(){
             $http.get('http://localhost:21395/api/contactsapp/Groups/0/1000').then(function success(response) {
                //debugger;
                $scope.groups = response.data.PayLoad;
            }, function error(response) {
                alert('something went wrong while fetching groups')
                console.log(response);
            });
        //}
        */



        $scope.AddContact = function() {
            debugger;
            $scope.newContact.groups = $scope.selectedGrpIds;
            

        }
        

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

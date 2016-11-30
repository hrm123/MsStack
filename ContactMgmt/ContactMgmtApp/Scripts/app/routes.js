(function(){
    var app = angular.module('ContactMgmtApp');
    app.config(routesConfig);

    
    routesConfig.$inject = ['$stateProvider', '$urlRouterProvider'];
    function routesConfig($stateProvider, $urlRouterProvider) {

    // Redirect to tab 1 if no other URL matches
    /* $urlRouterProvider.otherwise('/contacts'); */

    // Set up UI states
    $stateProvider
        .state('contacts', {
            url: '/contacts/',
            templateUrl: 'http://localhost:21395/home/list/',
            controller: 'ContactMgmtController',
            resolve: {
                    groupsList: ['ContactMgmtSvc', function (svc) {
                        return svc.getGroups();
                    }],
                    editContact: function () { return {}; }
                } 
        })

        .state('EditItem', {
            url: '/edit:id',
            templateUrl: function (stateParams) {
                return 'http://localhost:21395/home/editcontact/' + stateParams.id  ;
            },
            controller: 'ContactMgmtController' , 
            resolve: {
                groupsList: ['ContactMgmtSvc', function (svc) {
                    return svc.getGroups();
                }],
                editContact: ['ContactMgmtSvc','$stateParams', function (svc, $stateParams) {
                    return svc.getContact($stateParams.id);
                }],
            } 
        })
        .state('AddItem', {
            url: '/add',
            templateUrl: 'http://localhost:21395/home/addcontact/',
            controller: 'ContactMgmtController',
             resolve: {
                groupsList: ['ContactMgmtSvc', function (svc) {
                    return svc.getGroups();
                }],
                editContact: function () { return {}; }
            } 
        })

    }
    

})();
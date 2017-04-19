var app = angular.module('mainApp', []);

app.controller('indexController', ['$scope','$http', function($scope,$http){
    $scope.ShoppingLists = [];
    $scope.ShoppingList = {};
    $scope.id = 0;
    $scope.getAllLists = function () {
        $http.get("http://sync.jhonny.se/api/Values").then(function (result) {
            $scope.ShoppingLists = result.data;
            console.log("success: ", result);
        }, function (error) {
            console.log("fail: ",error);
        })
    };
    $scope.getList = function (id) {
        $http.get("http://sync.jhonny.se/api/Values/" + id).then(function (result) {
            $scope.ShoppingList = result.data;
            console.log("successfully got a list: ", result);
        }, function (error) {
            $scope.ShoppingList = {};
            console.log("fail: ", error);
        })
    }
}]);

app.controller('mainController', ['$scope', function ($scope) {
    $scope.Main = "Maincontroller";
}]);
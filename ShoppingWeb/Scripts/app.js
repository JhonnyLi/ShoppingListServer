var app = angular.module('mainApp', []);

app.controller('indexController', ['$scope','$http', function($scope,$http){
    $scope.ShoppingLists = [];
    $scope.ShoppingList = {};
    $scope.Items = [];
    $scope.id = 0;
    $scope.getAllLists = function () {
        $http.get("http://sync.jhonny.se/api/Values").then(function (result) {
        //$http.get("http://localhost:3768/api/Values").then(function (result) {
            $scope.ShoppingLists = result.data;
            console.log("success: ", result);
        }, function (error) {
            console.log("fail: ", error);
        });
    };
    $scope.getList = function (id) {
        $http.get("http://sync.jhonny.se/api/Values/" + id).then(function (result) {
        //$http.get("http://localhost:3768/api/Values/" + id).then(function (result) {
            $scope.ShoppingList = result.data;
            console.log("successfully got a list: ", result.data);
        }, function (error) {
            $scope.ShoppingList = {};
            console.log("fail: ", error);
        });
    };

    $scope.getAllItems = function () {
        console.log("Starts");
        $http.get("http://sync.jhonny.se/api/Items").then(function (result) {
        //$http.get("http://localhost:3768/api/Items").then(function (result) {
            console.log("Items success: ", result.data);
            $scope.Items = result.data;
        }, function (error) {
            console.log("Items fail: ", error);
        });
    };
    $scope.getAllItems();
}]);

app.controller('mainController', ['$scope', function ($scope) {
    $scope.Main = "Maincontroller";
}]);
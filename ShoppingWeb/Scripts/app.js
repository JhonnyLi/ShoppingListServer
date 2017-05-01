﻿var app = angular.module('mainApp', ['ngRoute']);

app.controller('mainController', ['$scope', '$route', '$routeParams', '$location',
    function ($scope, $route, $routeParams, $location) {
        var chat = "";
        $scope.preventRouteChange = false;
        $scope.location = $location.path();
        //In och utloggning
        $scope.logout = function () {
            $scope.userName = undefined;
            localStorage.username = undefined;
            $.connection.hub.stop();
            $scope.connected = false;
        };
        $scope.login = function () {
            $scope.userName = prompt('Enter your name: ', '');
            localStorage.username = $scope.userName;
            $scope.startHub();
            console.log(localStorage.username);
        };
        $scope.connected = false;
        $scope.connectedUsers = [];

        //Localstorage
        if (typeof (Storage) !== "undefined") {
            $scope.$parent.userName = localStorage.username;

        } else {
            console.log("No local storage support.");
            $scope.$parent.userName = "";
        }
        if ($scope.$parent.userName === undefined || $scope.$parent.userName === "") {
            $scope.login();
            $scope.$parent.userName = prompt('Enter your name: ', '');
            localStorage.username = $scope.$parent.userName;
        }
        //SignalR
        $scope.ChatMessages = [{ name: 'test', message: 'Message', timestamp: new Date() }, { name: 'test', message: 'Message 2', timestamp: new Date() }];
        var chatMessageObject = { name: "", message: "", timestamp: new Date() };
        $scope.startHub = function () {
            // Declare a proxy to reference the hub.
            chat = $.connection.syncHub;
            chat.state.userName = $scope.userName;
            // Create a function that the hub can call to broadcast messages.
            chat.client.broadcastMessage = function (name, message) {
                // Add the message to the page.
                var recievedMessage = angular.copy(chatMessageObject);
                recievedMessage.name = name;
                recievedMessage.message = message;
                $scope.ChatMessages.push(recievedMessage);
                $scope.$apply();
            };
            chat.client.connectionMessage = function (name,message) {
                var recievedMessage = angular.copy(chatMessageObject);
                recievedMessage.name = "Server";
                recievedMessage.message = message;
                $scope.ChatMessages.push(recievedMessage);
                $scope.connectedUsers.push(name);
                $scope.$apply();
            }
            chat.client.listMessage = function (name, list) {
                //Lägg till kod för att hantera listuppdateringar.
            };
            // Get the user name and store it to prepend to messages.
            // Set initial focus to message input box.
            $('#message').focus();
            // Start the connection.
            $.connection.hub.logging = true;
            //$.connection.hub.state.userName = "Jhonny";
            $.connection.hub.start($scope.userName).done(function () {
                $scope.connected = true;
                //chat.server.send($scope.userName, $scope.userName + " connected");
                chat.server.clientConnected();
                $scope.$apply();
                //$("#connected").text("Connected");
                //$('#sendmessage').click(function () {
                //    // Call the Send method on the hub.
                //    chat.server.send($scope.userName, $scope.chatMessage);
                //    // Clear text box and reset focus for next comment.
                //    $('#message').val('').focus();
                //});
            });
        };
        $scope.startHub();
        $scope.sendMessage = function () {
            // Call the Send method on the hub.
            chat.server.send($scope.userName, $scope.chatMessage);
            $scope.chatMessage = "";
            $('#message').focus();
            // Clear text box and reset focus for next comment.
            //$('#message').val('').focus();
        };
        $scope.SendOnEnter = function (event) {
            if (event.keyCode === 13) {
                $scope.sendMessage();
            }
        };
        $scope.navigate = function (path) {
            var navigateTo = "/" + path;
            $location.path(navigateTo);
            console.log($location);

            //location.href = '/index.html';
        };
        $scope.$on("$locationChangeStart", function (event) {
            if ($scope.preventRouteChange) {
                event.preventDefault();
            }
        });
        
    }]);

app.controller('indexController', ['$scope', '$http', '$location', '$rootScope',
    function ($scope,
        $http,
        $location,
        $rootScope) {
        $scope.$parent.Title = "Index";
        $scope.$parent.location = $location.path();
        //$scope.chatMessage = "";
        $scope.ShoppingLists = [];
        $scope.ShoppingList = {};
        $scope.Items = [];
        $scope.NumberOfLists = 0;
        $scope.getAllLists = function () {
            $http.get("http://sync.jhonny.se/api/Values").then(function (result) {
                //$http.get("http://localhost:3768/api/Values").then(function (result) {
                $scope.ShoppingLists = result.data;
                $scope.NumberOfLists = $scope.ShoppingLists.length > 0 ? $scope.ShoppingLists.length - 1 : $scope.ShoppingLists.length;
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
        $scope.getAllLists();
        $scope.getAllItems();

    }]);

app.controller('createController', ['$scope', '$http', '$location', '$rootScope',
    function ($scope, $http, $location, $rootScope) {
        $scope.$parent.location = $location.path();
        $scope.$parent.Title = "Create new list";
        $scope.newList = { ShoppingListId: "", CreatedDate: new Date(), Name: "", Items: [] };
        $scope.newItem = {};
        var itemObject = { ItemId: "", Name: "", Active: true };
        //angular.copy(itemObject, $scope.newItem);
        $scope.newItem = angular.copy(itemObject);
        $scope.addItem = function () {
            $scope.$parent.preventRouteChange = true;
            $scope.newList.Items.push($scope.newItem);
            $scope.newItem = angular.copy(itemObject);
        };
        $scope.addWithEnter = function (event) {
            event.preventDefault();
            console.log("AddWithEnter");
            if (event.keyCode === 13 && $scope.newItem.Name.length > 1) {
                $scope.addItem();
            }
        };
        $scope.reset = function () {
            $scope.newList.Name = "";
            $scope.newList.Items = [];
            $scope.$parent.preventRouteChange = false;
        };
        var currentDeleteTarget = {};
        $scope.removeItem = function (item, index, event) {
            currentDeleteTarget = item;
            $scope.newList.Items.splice(index, 1);
            event.cancelBubble = true;
        };
        $scope.toggleActive = function (item, event) {
            if (currentDeleteTarget != item) {

            }
        };
        
    }]);



app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: "partials/index.html",
        controller: "indexController"
    }).when('/api', {
        templateUrl: "partials/api.html"
    }).when('/create', {
        templateUrl: "partials/create.html",
        controller: "createController"
    })
    .otherwise({
        template: '<h1>Page does not exist</h1>'
    }
    )
});


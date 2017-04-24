var app = angular.module('mainApp', []);

app.controller('indexController', ['$scope','$http', function($scope,$http){
    var chat = "";
    $scope.userName = "";
    $scope.chatMessage = "";
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
    $scope.startHub = function () {
        // Declare a proxy to reference the hub.
        chat = $.connection.syncHub;
        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function (name, message) {
            // Html encode display name and message.
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        // Get the user name and store it to prepend to messages.
        //$('#displayname').val(prompt('Name:', ''));
        $scope.userName = prompt('Name:', '');
        // Set initial focus to message input box.
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function () {
            $("#connected").text("Connected");
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
        // Clear text box and reset focus for next comment.
        $('#message').val('').focus();
    }
}]);

app.controller('mainController', ['$scope', function ($scope) {
    $scope.Main = "Maincontroller";
}]);
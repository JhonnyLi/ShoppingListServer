var app = angular.module('mainApp', ['ngRoute']);
var local = false;
var address = local ? "http://localhost:3768" : "http://sync.jhonny.se";

window.fbAsyncInit = function () {
    FB.init({
        appId: '270392530055674',
        xfbml: true,
        version: 'v2.9'
    });
    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/sv_SE/sdk.js#xfbml=1&version=v2.9&appId=270392530055674";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

app.controller('mainController', ['$scope', '$route', '$routeParams', '$location', '$rootScope', '$http',
    function ($scope, $route, $routeParams, $location, $rootScope, $http) {
        var chat = $.connection.syncHub;
        $scope.preventRouteChange = false;
        var shoppingListCopy = {};
        $scope.ShoppingList = {};
        var itemObject = { ItemId: "", Name: "", Active: false, Deleted: false };
        angular.copy(itemObject, $scope.newItem);
        $scope.newItem = angular.copy(itemObject);
        $scope.location = $location.path();
        $scope.Auth = false;
        $scope.userTemplate = { FirstName: "", LastName: "", Password: "", Email: "" };
        $scope.user = angular.copy($scope.userTemplate);
        $scope.isAuthorized = function () {
            $http.post(address + "/Login/CheckLogin").then(function (result) {
                $scope.Auth = result.data;
                if ($scope.Auth) {
                    $scope.startHub();
                } else {
                    $scope.stopHub();
                }
            }, function (error) {
                $scope.Auth = false;
                console.log("fail: ", error);
            });
        };
        $scope.Login = function (user) {
            var dto = JSON.stringify($scope.user);
            $http.post(address + "/Login/Login", dto).then(function (result) {
                $scope.isAuthorized();
            }, function (error) {
                console.log("Login failed: ", error);
            });
            //}
        };
        $scope.FBLogin = function () {
            FB.login(function (response) {
                if (response.authResponse) {
                    console.log('Welcome!  Fetching your information.... ');
                    FB.api('/me', { fields: 'name,email,first_name,last_name' }, function (response) {
                        $scope.user.UserName = response.name;
                        $scope.SetLocalStorage(response);
                        $scope.FullUserName = response.name;
                        $scope.user.FirstName = response.first_name;
                        $scope.user.LastName = response.last_name;
                        $scope.user.Password = response.id;
                        $scope.user.Email = response.email;
                        $scope.Login();
                    });

                } else {
                    console.log('User cancelled login or did not fully authorize.');
                }
            }, { scope: 'public_profile,email' });
        };
        $scope.GetLocalStorage = function () {
            $scope.FullUserName = localStorage.username;
            $scope.user.FirstName = localStorage.first_name;
            $scope.user.LastName = localStorage.last_name;
            $scope.user.Password = localStorage.id;
            $scope.user.Email = localStorage.email;
        };
        $scope.SetLocalStorage = function (response) {
            localStorage.username = response.name;
            localStorage.first_name = response.first_name;
            localStorage.last_name = response.last_name;
            localStorage.id = response.id;
            localStorage.email = response.email;
        };
        $scope.ClearLocalStorage = function (response) {
            localStorage.username = "";
            localStorage.first_name = "";
            localStorage.last_name = "";
            localStorage.id = "";
            localStorage.email = "";
            $scope.GetLocalStorage();
        };

        //$scope.$watch('ShoppingList', function () {
        //    $scope.CheckIfItemListIsPristine();


        //}, true); //true säger till $watch att kolla på alla attribut i objektet.
        if (!local) {
            $scope.isAuthorized();
        }
        $scope.startHub = function () {
            console.log("starthub: ", $scope.Auth);
            if ($scope.Auth) {
                chat.state.userName = $scope.user.UserName;

                chat.client.broadcastMessage = function (name, message) {
                    var recievedMessage = angular.copy(chatMessageObject);
                    recievedMessage.name = name;
                    recievedMessage.message = message;
                    $scope.ChatMessages.push(recievedMessage);
                    $scope.$apply();
                };
                chat.client.connectionMessage = function (name, message) {
                    var recievedMessage = angular.copy(chatMessageObject);
                    recievedMessage.name = "Server";
                    recievedMessage.message = message;
                    $scope.ChatMessages.push(recievedMessage);
                    $scope.$apply();
                };
                chat.client.listMessage = function (name, list) {
                    console.log("listeMessage yo !");
                    $scope.ShoppingList = list;
                    $scope.$apply();
                    var recievedMessage = angular.copy(chatMessageObject);
                    recievedMessage.name = "Server";
                    recievedMessage.message = name + "updated the list";
                    $scope.ChatMessages.push(recievedMessage);
                };
                chat.client.usersOnlineMessage = function (users) {
                    $scope.connectedUsers = JSON.parse(users);
                    console.log("UserList updated");
                };
                chat.client.userList = function (list) {
                    $scope.connectedUsers = list;
                };
                // Get the user name and store it to prepend to messages.
                // Set initial focus to message input box.
                $('#message').focus();
                // Start the connection.
                $.connection.hub.logging = true;
                $.connection.hub.qs = { 'username': $scope.user.UserName };
                $.connection.hub.start($scope.user.UserName).done(function () {

                }).done(function () { $scope.connected = true; });
            }
        };
        $scope.stopHub = function () {
            $.connection.hub.stop();
        };
        $scope.SendUpdatedList = function () {
            if ($scope.connected) {
                $scope.ShoppingList.ListUpdated = true;
                var jsonlist = JSON.stringify($scope.ShoppingList);
                chat.server.sendList(jsonlist).done(function () {
                    $scope.ShoppingList.ListUpdated = false;
                });
            }
        };
        //In och utloggning
        $scope.logout = function () {
            $scope.userName = undefined;
            localStorage.username = undefined;
            $.connection.hub.stop();
            $scope.connected = false;
        };
        $scope.login = function () {
            localStorage.username = $scope.user.UserName;
            if ($scope.Auth) {
                $scope.startHub();
            }
        };
        $scope.connected = false;
        $scope.connectedUsers = [];
        $scope.GetLocalStorage();
        //Localstorage
        if (typeof Storage !== "undefined") {
            $scope.user.UserName = localStorage.username;

        } else {
            console.log("No local storage support.");
            $scope.$parent.userName = "";
        }
        if ($scope.user.UserName === undefined || $scope.user.UserName === "") {
            $scope.login();
            localStorage.username = $scope.user.UserName;
        }
        //SignalR
        $scope.ChatMessages = [];
        var chatMessageObject = { name: "", message: "", timestamp: new Date() };

        $scope.startHub();
        $scope.RefreshConnection = function () {
            console.log("MainController");
            $scope.stopHub();
            $scope.startHub();
        };
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
        $scope.SaveListUpdate = function () {
            console.log('ShoppingList has changed!');
            shoppingListCopy = angular.copy($scope.ShoppingList);
            $scope.ShoppingList.ListUpdated = true;
            $scope.SendUpdatedList();
            //}
        };
        $scope.DeleteItem = function (item) {
            item.Deleted = true;
            console.log(item);
        };
        $scope.navigate = function (path) {
            var navigateTo = "/" + path;
            $location.path(navigateTo);
        };
        $scope.$on("$locationChangeStart", function (event) {
            if ($scope.preventRouteChange) {
                event.preventDefault();
            }
        });
        $scope.fakeLogin = function () {
            $scope.user.UserName = "Fake Login";
            var response = { name: "admin@jhonny.se", first_name: "Fake", last_name: "", id: "1Schema77!", email: "admin@jhonny.se" };
            $scope.SetLocalStorage(response);
            $scope.GetLocalStorage();
            $scope.Auth = true;
            $scope.Login();
            $scope.startHub();
        };
    }]);

app.controller('indexController', ['$scope', '$http', '$location', '$rootScope',
    function ($scope,
        $http,
        $location,
        $rootScope) {
        $scope.$parent.Title = "Index";
        $scope.$parent.location = $location.path();
        $scope.Items = [];
        var newItemTemplate = { Name: "", Comment: "", Active: false, Deleted: false };
        var newItem = angular.copy(newItemTemplate);

        $scope.NumberOfLists = 0;

        $scope.AddItem = function () {
            console.log("AddItem: ", $scope.$parent.ShoppingList);
            $scope.$parent.ShoppingList.Items.push($scope.newItem);
            $scope.newItem = angular.copy(newItemTemplate);
        };
        $scope.AddItemOnEnter = function (event) {
            if (event.keyCode === 13) {
                $scope.AddItem();
            }
        };
    }]);

app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: "partials/index.html",
        controller: "indexController"
    }).when('/login', {
        templateUrl: "partials/login.html",
        controller: "loginController"
    }).otherwise({
        template: '<h1>Page does not exist</h1>'
    }
    );
});


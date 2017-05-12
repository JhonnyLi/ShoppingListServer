var app = angular.module('mainApp', ['ngRoute']);
//var app = angular.module('mainApp', []);
//var chat = $.connection.syncHub;

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
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

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
        $scope.location = $location.path();
        $scope.Auth = false;
        $scope.userTemplate = { FirstName: "", LastName: "", Password: "", Email: "" };
        $scope.user = angular.copy($scope.userTemplate);
        $scope.justLogShit = function () {
            console.log($scope.user);
        };
        $scope.isAuthorized = function () {
            //http://sync.jhonny.se/api/Values
            //$http.post("http://localhost:3768/Login/CheckLogin").then(function (result) {
            $http.post("http://sync.jhonny.se/Login/CheckLogin").then(function (result) {
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
            console.log("Login starts");
            var dto = JSON.stringify($scope.user);
            //dto = $scope.user;
            console.log("Json object: ", dto);
            //$http.post("http://localhost/Login/Login", dto).then(function (result) {
            //$http.post("http://localhost:3768/Login/Login", dto).then(function (result) {
            $http.post("http://sync.jhonny.se/Login/Login",dto).then(function (result) {
                //$scope.user.UserName = result.data.UserName;
                //$scope.user.FirstName = result.data.first_name;
                //$scope.user.LastName = result.data.last_name;
                $scope.isAuthorized();
                //$scope.navigate('');
                console.log("Login successful: ", result);
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
                        //$scope.user.UserName = "Test";
                        var fname = response.first_name;
                        debugger;
                        $scope.user.FirstName = response.first_name;
                        $scope.user.LastName = response.last_name;
                        $scope.user.Password = response.id;
                        $scope.user.Email = response.email;
                        $scope.Login()
                    });
                    
                } else {
                    console.log('User cancelled login or did not fully authorize.');
                }
            }, { scope: 'public_profile,email' });
        }

        

        $scope.$watch('Auth', function () {
            if (!$scope.Auth) {
                //Show loginscreen magics.
            }
        });
        $scope.isAuthorized();
        $scope.startHub = function () {
            console.log("starthub: ", $scope.Auth);
            if ($scope.Auth) {
                // Declare a proxy to reference the hub.
                //chat = $.connection.syncHub;
                chat.state.userName = $scope.user.UserName;

                // Create a function that the hub can call to broadcast messages.
                chat.client.broadcastMessage = function (name, message) {
                    // Add the message to the page.
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
                    $scope.connectedUsers.push(name);
                    $scope.$apply();
                };
                chat.client.listUpdateMessage = function (list) {
                    var updatedList = JSON.parse(list);
                    $scope.ShoppingList.Items = updatedList;
                    var recievedMessage = angular.copy(chatMessageObject);
                    recievedMessage.name = "Server";
                    recievedMessage.message = "List updated";
                    $scope.ChatMessages.push(recievedMessage);
                }
                chat.client.contextMessage = function (serial) {

                    var x = JSON.parse(serial);
                    debugger;

                };
                chat.client.userList = function (list) {
                    $scope.connectedUsers = list;
                }
                chat.client.listMessage = function (name, list) {
                    //Lägg till kod för att hantera listuppdateringar.
                };
                // Get the user name and store it to prepend to messages.
                // Set initial focus to message input box.
                $('#message').focus();
                // Start the connection.
                $.connection.hub.logging = true;
                $.connection.hub.qs = { 'username': $scope.user.UserName };
                //chat.qs = { 'username': 'Jhonny' };
                //$.connection.hub.state.userName = "Jhonny";
                $.connection.hub.start($scope.user.UserName).done(function () {
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
        };
        $scope.stopHub = function () {
            $.connection.hub.stop();
        }
        //In och utloggning
        $scope.logout = function () {
            $scope.userName = undefined;
            localStorage.username = undefined;
            $.connection.hub.stop();
            $scope.connected = false;
        };
        $scope.login = function () {
            $scope.userName = prompt('Enter your name: ', '');
            localStorage.username = $scope.user.UserName;
            if ($scope.Auth) {
                $scope.startHub();
            }
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
        $scope.ChatMessages = [];
        var chatMessageObject = { name: "", message: "", timestamp: new Date() };

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
            }, function (error) {
                $scope.ShoppingList = {};
                console.log("fail: ", error);
            });
        };
        $scope.getAllItems = function () {
            $http.get("http://sync.jhonny.se/api/Items").then(function (result) {
                //$http.get("http://localhost:3768/api/Items").then(function (result) {
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
            //debugger;
            if ($scope.newItem.Name !== "") {
                $scope.$parent.preventRouteChange = true;
                $scope.newList.Items.push($scope.newItem);
                $scope.newItem = angular.copy(itemObject);
            }
        };
        $scope.addWithEnter = function (event) {
            event.preventDefault();
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
        $scope.saveList = function (event) {
            //var x = document.activeElement;
            //if (x.id === "savebutton") {
            var dto = JSON.stringify($scope.newList);
            //$http.post("http://localhost:3768/api/Values", dto).then(function (result) {
            $http.post("http://sync.jhonny.se/api/Values", dto).then(function (result) {
                //$http.get("http://localhost:3768/api/Values").then(function (result) {
                $scope.ShoppingLists = result.data;
                $scope.NumberOfLists = $scope.ShoppingLists.length > 0 ? $scope.ShoppingLists.length - 1 : $scope.ShoppingLists.length;
                console.log("success: ", result);
            }, function (error) {
                console.log("fail: ", error);
            });
            //}
        };
        $scope.toggleActive = function (item, event) {
            if (currentDeleteTarget !== item) {
                //delete ?
            }
        };

    }]);

app.controller('loginController', ['$scope', '$http', '$location', '$rootScope',
    function ($scope, $http, $location, $rootScope) {

        function test(response) {
            console.log("test:", response);
        }
        //FB.getLoginStatus(function (response) {
        //    console.log("Facebook says: ", response);
        //    //statusChangeCallback(response);
        //    test(response);
        //});

        $scope.$parent.location = $location.path();
        $scope.$parent.Title = "Login";

        $scope.navigate = function (path) {
            var navigateTo = "/" + path;
            $location.path(navigateTo);
            console.log($location);
        };
        $scope.Login = function (user) {
            //var x = document.activeElement;
            //if (x.id === "savebutton") {
            //$http.post("http://localhost:3768/api/Values", dto).then(function (result) {
            //$http.post("http://sync.jhonny.se/api/Values", dto).then(function (result) {
            var dto = JSON.stringify($scope.$parent.user);
            $http.post("http://sync.jhonny.se/Login/Login", dto).then(function (result) {
                //$http.post("http://localhost:3768/Login/Login", dto).then(function (result) {
                //$http.get("http://localhost:3768/api/Values").then(function (result) {
                $scope.$parent.user.UserName = result.data.UserName;
                $scope.$parent.isAuthorized();
                $scope.navigate('');
                console.log("success: ", result);
            }, function (error) {
                console.log("fail: ", error);
            });
            //}
        };



    }]);

app.config(function ($routeProvider) {
    $routeProvider.when('/', {
        templateUrl: "partials/index.html",
        controller: "indexController"
    }).when('/login', {
        templateUrl: "partials/login.html",
        controller: "loginController"
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


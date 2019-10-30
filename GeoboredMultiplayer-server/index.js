var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

let connectedClients = [];

/* 
Mongo DB connection

var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/admin";
var dataBase;
*/

io.on('connect', function(socket) {

    console.log("test");

    socket.on('join', function(client) {
        connectedClients.push(client);
        BroadCastToClients('join', client.id);
        console.log(client.id + " Joined the game");
    });
    
    socket.on('quit', function(client){
        connectedClients.push(client);
        BroadCastToClients('quit', client.id);
        console.log(client + " left the game");
    });
    
    socket.on('move', function(data){
        console.log("test");
        //BroadCastToClients('move', data);
    });

});

function BroadCastToClients(functionName, data) {
    connectedClients.forEach(client => {
        client.emit(functionName, data); 
    });
}

http.listen(3000, function() {
    //Announce we're live
    console.log('listening on *:3000');
});

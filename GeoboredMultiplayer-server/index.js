var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

let connectedSockets = [];
let connectedPlayers = [];
let bullets = [];

/* 
Mongo DB connection

var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/admin";
var dataBase;
*/

io.on('connect', function(socket) {

    socket.on('join', (data) => {
        connectedSockets.push(socket);
        connectedPlayers.push(new Player(socket.id, data.x, data.y, data.rotX, data.rotY));
        var dataObject = new JoinPacket(socket.id, data.x, data.y, data.rotX, data.rotY);

        //We need to send the new player all the other players! Otherwise he will be alone forever!!
        connectedPlayers.forEach(player => {
            socket.emit('join', player);
        });

        BroadCastToClients('join', dataObject);
        console.log(dataObject.clientId + " Joined the game");
    });
    
    socket.on('disconnect', () => {
        var index = 0;
        connectedSockets.forEach(currentSocket => {
            if(socket == currentSocket){
                connectedSockets.splice(index);
            }
            index++;
        });

        index = 0;
        connectedPlayers.forEach(player => {
            if(player.clientId == socket.id){
                connectedPlayers.splice(index);
            }
            index++;
        });

        var dataObject = new QuitPacket(socket.id);
    
        BroadCastToClients('quit', dataObject);
        console.log(dataObject.clientId + " left the game");
    });
    
    socket.on('move', function(data) {
        connectedPlayers.forEach(player => {
            if(player.clientId == data.clientId){
                player.x = parseFloat(data.x);
                player.y = parseFloat(data.y);
                player.rotZ = parseFloat(data.rotZ);
                player.rotY = parseFloat(data.rotY);
            }
        });
    
        BroadCastToClients('move', data);
    });

    socket.on('move_bullet', function(data) {
        if(data.spawn) {
            bullets.push(new Bullet(socket.id, data.x, data.y, data.rotX, data.rotY));
        } else {
            bullets.forEach(bullet => {
                if(bullet.clientId == data.clientId){
                    bullet.x = parseFloat(data.x);
                    bullet.y = parseFloat(data.y);
                    bullet.rotZ = parseFloat(data.rotZ);
                    bullet.rotY = parseFloat(data.rotY);
                }
            });
        }

        BroadCastToClients('move_bullet', data);
    });

});

function BroadCastToClients(functionName, data) {
    connectedSockets.forEach(socket => {
        socket.emit(functionName, data); 
    });
}

http.listen(3000, function() {
    //Announce we're live
    console.log('listening on *:3000');
});

/* Server sided data objects (can be used as packets) */
class Player {
    constructor(clientId, x, y, rotZ, rotY) {
        this.clientId = clientId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
    }
}

class Bullet {
    constructor(clientId, x, y, rotZ, rotY) {
        this.clientId = clientId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
    }
}

/* Packets */

class JoinPacket {
    constructor(clientId, x, y, rotZ, rotY) {
        this.clientId = clientId;
        this.x = x;
        this.y = y;
        this.rotZ = rotZ;
        this.rotY = rotY;
    }
}

class QuitPacket {
    constructor(clientId) {
        this.clientId = clientId;
    }
}
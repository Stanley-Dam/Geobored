var express = require('express');
var app = express();
var http = require('http').Server(app);
var io = require('socket.io')(http);

let connectedPlayers = [];

/* 
Mongo DB connection

var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/admin";
var dataBase;
*/

io.on('connect', function(socket) {

    socket.on('join', (data) => {
        var newPlayer = new Player(socket.id, data.x, data.y, data.rotY, data.rotZ);
        connectedPlayers.push(newPlayer);

        //We need to send the new player all the other players! Otherwise he will be alone forever!!
        if(connectedPlayers != []) {
            connectedPlayers.forEach(player => {
                socket.emit('join', player);
            });
        }

        BroadCastToClients('join', newPlayer);
        console.log(newPlayer.clientId + " Joined the game");
        
    });
    
    socket.on('disconnect', () => {
<<<<<<< Updated upstream
        connectedSockets.splice(socket);
=======
        index = 0;
>>>>>>> Stashed changes
        connectedPlayers.forEach(player => {
            if(player.clientId == socket.id){
                connectedPlayers.splice(player);
            }
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
            }
        });
    
        BroadCastToClients('move', data);
    });

<<<<<<< Updated upstream
=======
    socket.on('bullet_spawn', function(data) {
        bullets.push(new Bullet(socket.id, data.bulletUUID, data.x, data.y, data.rotY, data.rotZ));
        BroadCastToClients('bullet_spawn', data);
    });

    socket.on('bullet_hit', function(data) {
        var index = 0;
        bullets.forEach(bullet => {
            if(bullet.clientId == data.clientId && data.bulletUUID == bullet.bulletUUID){
                bullets.splice(index);
            }
            index++;
        });

        BroadCastToClients('bullet_hit', data);
    });

>>>>>>> Stashed changes
});

function BroadCastToClients(functionName, data) {
    io.sockets.clients().emit(functionName, data);
}

http.listen(3000, function() {
    //Announce we're live
    console.log('listening on *:3000');
});

/* Server sided data objects (can be used as packets) */
class Player {
<<<<<<< Updated upstream
    constructor(clientId, x, y, rotX, rotY) {
=======
    constructor(clientId, x, y, rotY, rotZ) {
        this.clientId = clientId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
    }
}

class Bullet {
    constructor(clientId, bulletUUID, x, y, rotZ, rotY) {
>>>>>>> Stashed changes
        this.clientId = clientId;
        this.bulletUUID = bulletUUID;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotX = parseFloat(rotX);
        this.rotY = parseFloat(rotY);
    }
}

/* Packets */

<<<<<<< Updated upstream
class JoinPacket {
    constructor(clientId, x, y, rotX, rotY) {
        this.clientId = clientId;
        this.x = x;
        this.y = y;
        this.rotX = rotX;
        this.rotY = rotY;
    }
}

=======
>>>>>>> Stashed changes
class QuitPacket {
    constructor(clientId) {
        this.clientId = clientId;
    }
}
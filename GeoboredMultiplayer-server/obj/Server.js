const Player = require('./Player.js');
const Bullet = require('./Bullet.js');
const QuitPacket = require('../packets/QuitPacket.js');

class Server {
    constructor(proxy, serverId, port) {
        this.proxy = proxy;
        this.serverId = serverId;
        this.connectedPlayers = [];
        this.bullets = [];
        this.port = port;

        this.io = require('socket.io')({
            transports: ['websocket'],
        });

        this.io.attach(this.port);
        console.log("Started listening on: " + this.port);

        //Start updating the server
        this.ServerLoop(this);
    }

    ServerLoop(server) {
        this.io.on('connect', function(socket) {
            socket.on('join', (data) => {
                var newPlayer = new Player(socket.id, data.playerTypeId, data.x, data.y, data.rotY, data.rotZ);
                server.connectedPlayers.push(newPlayer);
        
                //We need to send the new player all the other players! Otherwise he will be alone forever!!
                server.connectedPlayers.forEach(player => {
                    socket.emit('join', player);
                });
        
                server.BroadCastToClients('join', newPlayer);
                console.log(newPlayer.clientId + " Joined the game (Server: " + server.port + ")");
                
            });
            
            socket.on('disconnect', () => {
                var index = 0;
                server.connectedPlayers.forEach(player => {
                    if(player.clientId == socket.id) {
                        server.connectedPlayers.splice(index);
                    }
                    index++;
                });
        
                var dataObject = new QuitPacket(socket.id);
            
                server.BroadCastToClients('quit', dataObject);
                console.log(dataObject.clientId + " left the game");

                if(server.connectedPlayers.length <= 0) {
                    server.proxy.DestroyServer(server);
                    server.io.close();
                    return;
                }
            });
            
            socket.on('move', function(data) {
                server.connectedPlayers.forEach(player => {
                    if(player.clientId == data.clientId){
                        player.x = parseFloat(data.x);
                        player.y = parseFloat(data.y);
                        player.rotZ = parseFloat(data.rotZ);
                        player.rotY = parseFloat(data.rotY);
                    }
                });
            
                server.BroadCastToClients('move', data);
            });
        
            socket.on('bullet_spawn', function(data) {
                server.bullets.push(new Bullet(socket.id, data.bulletUUID, data.bulletTypeId, data.x, data.y, data.rotY, data.rotZ));
                server.BroadCastToClients('bullet_spawn', data);
            });
        
            socket.on('bullet_hit', function(data) {
                var index = 0;
                server.bullets.forEach(bullet => {
                    if(bullet.clientId == data.clientId && data.bulletUUID == bullet.bulletUUID){
                        bullets.splice(index);
                    }
                    index++;
                });
        
                server.BroadCastToClients('bullet_hit', data);
            });
        });
    }

    BroadCastToClients(functionName, data) {
        this.io.sockets.clients().emit(functionName, data);
    }
}

module.exports = Server;
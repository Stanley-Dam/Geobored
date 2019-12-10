const Player = require('./Player.js');
const Bullet = require('./Bullet.js');
const QuitPacket = require('../packets/QuitPacket.js');
const PlayerDeathPacket = require('../packets/PlayerDeath.js');
const PlayerHealthUpdate = require('../packets/PlayerHealthUpdate.js');

class Server {
    constructor(proxy, serverId, port) {
        this.proxy = proxy;
        this.serverId = serverId;
        this.connectedPlayers = [];
        this.bullets = [];
        this.port = port;

        this.gameStarted = false;

        this.io = require('socket.io')({
            transports: ['websocket'],
        });

        this.io.attach(this.port);
        console.log("Started listening on: " + this.port);

        //Start updating the server
        this.ServerLoop(this);
    }

    ServerLoop(server) {
        this.io.on('connection', function(socket) {
            socket.on('join', (data) => {
                var newPlayer = new Player(socket.id, data.playerTypeId, data.x, data.y, data.rotY, data.rotZ, data.HP);
                server.connectedPlayers.push(newPlayer);
        
                //We need to send the new player all the other players! Otherwise he will be alone forever!!
                server.connectedPlayers.forEach(player => {
                    socket.emit('join', player);
                });
        
                server.BroadCastToClients('join', newPlayer);
                console.log(newPlayer.clientId + " Joined the game (Server: " + server.port + ")");
            
                if(server.connectedPlayers.length >= 2)
                    server.Start();
            });
            
            socket.on('disconnect', function() {
                var index = 0;
                server.connectedPlayers.forEach(player => {
                    if(player.clientId == socket.id) {
                        server.connectedPlayers.splice(index);
                        server.CheckForEndGame();
                    }
                    index++;
                });
        
                var dataObject = new QuitPacket(socket.id);
            
                server.BroadCastToClients('quit', dataObject);
                console.log(dataObject.clientId + " left the game(Server: " + server.port + ")");

                if(server.connectedPlayers.length <= 0) {
                    server.proxy.DestroyServer(server);
                    server.Stop();
                }
            });
            
            socket.on('takeDamage', function(data) {
                var playerHP = 0;

                server.connectedPlayers.forEach(player => {
                    if(data.clientId == player.clientId) {
                        playerHP = player.TakeDamage(server, data.damage);

                        if(player.isDeath)
                            server.CheckForEndGame();
                    }
                });

                server.BroadCastToClients('setPlayerHealth', new PlayerHealthUpdate(data.clientId, playerHP));
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
                if(server.gameStarted) {
                    server.bullets.push(new Bullet(socket.id, data.bulletUUID, data.bulletTypeId, data.x, data.y, data.rotY, data.rotZ, data.damage, data.speed));
                    server.BroadCastToClients('bullet_spawn', data);
                }
            });
        
            socket.on('bullet_hit', function(data) {
                var index = 0;
                server.bullets.forEach(bullet => {
                    if(bullet.clientId == data.clientId && data.bulletUUID == bullet.bulletUUID){
                        bullets.splice(index);
                    }
                    index++;
                });
            });
        });
    }

    Sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    Start() {
        this.Sleep(500);
        console.log("Started! " + this.port);
        this.gameStarted = true;
    }

    Stop() {
        this.io.close();
    }

    BroadCastToClients(functionName, data) {
        this.io.sockets.clients().emit(functionName, data);
    }

    async MessageToClients(functionName) {
        this.io.sockets.clients().emit(functionName);
    }

    async CheckForEndGame() {
        var aliveIndex = 0;
        this.connectedPlayers.forEach(player => {
            if(!player.isDeath)
                aliveIndex++;
        });

        if(aliveIndex <= 1) {
            await this.MessageToClients("serverStopped");
            this.proxy.DestroyServer(this);
            this.io.close();
        }
    }
}

module.exports = Server;
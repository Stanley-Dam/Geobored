//Imports
const RedirectPacket = require('./packets/RedirectPacket.js');
const Server = require('./obj/Server.js');

/* 
Mongo DB connection

var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/admin";
var dataBase;
*/
class Proxy {
    constructor() {
        this.servers = [];

        this.io = require('socket.io')({
            transports: ['websocket'],
        });

        this.io.attach(3000);
        console.log('listening on *:3000');

        this.Run(this);
    }

    Run(proxy) {
        this.io.on('connect', function(socket) {
            socket.on('join', (data) => {
                var joinedExistingGame = false;

                if(proxy.servers.length > 0) {
                    proxy.servers.forEach(server => {
                        if(server.connectedPlayers.length < 4 || server.gameStarted) {
                            socket.emit('redirectToServer', new RedirectPacket(server.port));
                            joinedExistingGame = true;
                        }
                    });
                }

                if(!joinedExistingGame) {
                    var newServer = new Server(proxy, 0, (1000 + Math.floor(Math.random() * 8999)));
                    proxy.servers.push(newServer);
                    socket.emit('redirectToServer', new RedirectPacket(newServer.port));
                }
            });
        }); 
    }

    DestroyServer(server) {
        var index = 0;
        this.servers.forEach(currentServer => {
            if(currentServer == server) {
                console.log(server.port + " has been disabled!");
                this.servers.splice(index);
            }
            index++;
        });

        server.Stop();
    }
}

new Proxy();
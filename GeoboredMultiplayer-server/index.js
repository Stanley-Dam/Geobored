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
                if(proxy.servers.length <= 0)
                    proxy.servers.push(new Server(proxy, 0, (1000 + Math.floor(Math.random() * 8999))));

                socket.emit('redirectToServer', new RedirectPacket(proxy.servers[Math.floor(Math.random() * proxy.servers.length)].port));
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
    }
}

new Proxy();
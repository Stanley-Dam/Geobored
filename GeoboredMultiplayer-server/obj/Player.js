const PlayerDeathPacket = require('../packets/PlayerDeath.js');

class Player {
    constructor(clientId, playerTypeId, x, y, rotY, rotZ, HP) {
        this.clientId = clientId;
        this.playerTypeId = playerTypeId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
        this.HP = HP;
        this.isDeath = false;
    }

    TakeDamage(server, damageAmount) {
        this.HP -= damageAmount;

        if(this.HP <= 0) {
            //Player is dead
            server.BroadCastToClients('playerDeath', new PlayerDeathPacket(this.clientId));
            this.isDeath = true;
        }

        return this.HP;
    }
}

module.exports = Player;
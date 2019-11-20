class Player {
    constructor(clientId, playerTypeId, x, y, rotY, rotZ) {
        this.clientId = clientId;
        this.playerTypeId = playerTypeId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
    }
}

module.exports = Player;
class Bullet {
    constructor(clientId, bulletUUID, bulletTypeId, x, y, rotZ, rotY) {
        this.clientId = clientId;
        this.bulletUUID = bulletUUID;
        this.bulletTypeId = bulletTypeId;
        this.x = parseFloat(x);
        this.y = parseFloat(y);
        this.rotZ = parseFloat(rotZ);
        this.rotY = parseFloat(rotY);
    }
}

module.exports = Bullet;
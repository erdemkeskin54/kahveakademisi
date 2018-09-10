import { Injectable } from '@angular/core';


import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { JwtPacket } from '../../entities/request-models/Auth/jwt-packet';



@Injectable()
export class UserService {

    public whoami: JwtPacket;

    constructor() {

    }

    setUser(jwtPacket: JwtPacket) {
        this.whoami = jwtPacket;
    }

    getUser(): JwtPacket {
        if (this.whoami) {
            return this.whoami;
        }
        else {
            return this.whoami = new JwtPacket();
        }

    }

}
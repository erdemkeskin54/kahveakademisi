export class JwtPacket{
    token:string=null;
    userName: string=null;
    firstName: string=null;
    lastName: string=null;
    expiration:string=null;

    /**
     *
     */
    constructor(token?,userName?,firstName?,lastName?,expiration?) {    

        this.token=token;
        this.userName=userName;
        this.firstName=firstName;
        this.lastName=lastName;
        this.expiration=expiration;

    }
}
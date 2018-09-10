/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent SignIn Component
 * File path - '../../../src/pages/sign-in/sign-in'
 */

import { Component } from '@angular/core';
import { IonicPage, NavController, MenuController } from 'ionic-angular';
import { ProductService } from "../../providers/product/product.service";
import { Product } from '../../entities/response-models/product';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { AuthService } from '../../providers/auth/auth.service';
import { Login } from '../../entities/request-models/Auth/login';
import { JwtPacket } from '../../entities/request-models/Auth/jwt-packet';
import { UserService } from '../../providers/auth/user.service';
import { SocketService } from '../../providers/socket/socket.service';

@IonicPage()
@Component({
  selector: 'page-sign-in',
  templateUrl: 'sign-in.html',
})
export class SignInPage {

  response: Product[];
  apiResponse: ApiResponse;
  jwtPacket:JwtPacket;

  constructor(public navCtrl: NavController,
    public menuCtrl: MenuController,
    private authService: AuthService,
    private userService:UserService,
    private socketService:SocketService) {
    this.menuCtrl.enable(false); // Disable SideMenu

  }

  ionViewDidLoad() {

  }

  gotoForgetPasswordPage() {
    this.navCtrl.setRoot('ForgetPasswordPage');
  }

  gotoSignUpPage() {
    this.navCtrl.setRoot('SignUpPage');
  }

  signIn() {
   
    let loginModel=new Login("05533607714","411811c");
    this.authService.login(loginModel).subscribe(x=>{
      this.apiResponse=x;
      console.log(x);
      if(this.apiResponse.isSucces==true)
      {
        this.userService.setUser(this.apiResponse.returnObject);
        //start socket connection
        this.socketService._connect();
        this.navCtrl.setRoot('HomePage');
      }
      else
      {
        this.apiResponse.errorContent.forEach(element => {
          console.log(element.errorCode+ " : "+element.errorMessage);
        });
      }
    });
  }
}

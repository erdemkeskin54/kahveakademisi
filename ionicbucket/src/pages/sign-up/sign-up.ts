/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent SignUp Component
 * File path - '../../../src/pages/sign-up/sign-up'
 */

import { Component } from '@angular/core';
import { IonicPage, NavController, MenuController } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-sign-up',
  templateUrl: 'sign-up.html',
})
export class SignUpPage {

  constructor(public navCtrl: NavController,
    public menuCtrl: MenuController) {
    this.menuCtrl.enable(false); // Disable SideMenu
  }

  // SignIn Action
  gotoSignInPage() {
    this.navCtrl.setRoot('SignInPage');
  }

  // SignUp Action
  signUp() {
    this.navCtrl.setRoot('HomePage');
  }
}

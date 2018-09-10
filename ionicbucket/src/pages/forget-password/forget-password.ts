/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Forget Password Component
 * File path - '../../../src/pages/forget-password/forget-password'
 */


import { Component } from '@angular/core';
import { IonicPage, NavController, MenuController } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-forget-password',
  templateUrl: 'forget-password.html',
})
export class ForgetPasswordPage {

  constructor(public navCtrl: NavController,
    public menuCtrl: MenuController) {
    this.menuCtrl.enable(false); // Disable Side Menu
  }

  // Submit Button
  submit() {
    this.navCtrl.setRoot('SignInPage');
  }
}

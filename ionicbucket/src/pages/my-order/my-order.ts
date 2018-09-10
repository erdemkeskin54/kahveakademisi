/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent My Order Page Component
 * File path - '../../../src/pages/my-order/my-order'
 */

import { Component } from '@angular/core';
import { IonicPage, MenuController } from 'ionic-angular';

@IonicPage()
@Component({
  selector: 'page-my-order',
  templateUrl: 'my-order.html',
})
export class MyOrderPage {

  segment: any;

  constructor(
    public menuCtrl: MenuController) {
    this.segment = "pending"; // Default Segment
    this.menuCtrl.enable(true); // Enable Side Menu
  }
}

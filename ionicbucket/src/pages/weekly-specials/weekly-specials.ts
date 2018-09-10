/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Weekly Specials Component
 * File path - '../../../src/pages/weekly-specials/weekly-specials'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';

@IonicPage()
@Component({
  selector: 'page-weekly-specials',
  templateUrl: 'weekly-specials.html',
})
export class WeeklySpecialsPage {

  /**
   * List of all weekly Product Items
   */
  listItems: any = [];

  constructor(
    public dataProvider: DataProvider,
    public modalCtrl: ModalController) {
  }

  ionViewDidLoad() {
    this.getProducts();
  }

  // Get weekly Products
  getProducts() {
    this.dataProvider.getProducts().subscribe((data: any) => {
      this.listItems = data;
    });
  }

  // Open Product Details Page
  openProductDetails(product) {
    this.modalCtrl.create('ProductDetailsPage', { product: product }).present();
  }
}

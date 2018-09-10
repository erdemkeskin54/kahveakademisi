/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent New Product Component
 * File path - '../../../src/pages/new-products/new-products'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';

@IonicPage()
@Component({
  selector: 'page-new-products',
  templateUrl: 'new-products.html',
})
export class NewProductsPage {
  productList: any = [];
  constructor(
    public dataProvider: DataProvider,
    public modalCtrl: ModalController) {
  }

  ionViewDidLoad() {
    this.getProducts();
  }

  /**
   * Get List Of Product
   */
  getProducts() {
    this.dataProvider.getProducts().subscribe((data: any) => {
      this.productList = data;
    });
  }

  /**
   * @method openProductDetails 
   * @param product 
   */
  openProductDetails(product) {
    this.modalCtrl.create('ProductDetailsPage', { product: product }).present();
  }

}

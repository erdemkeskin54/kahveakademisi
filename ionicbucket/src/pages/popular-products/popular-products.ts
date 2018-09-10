/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Popular Product Component
 * File path - '../../../src/pages/popular-products/popular-products'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';

@IonicPage()
@Component({
  selector: 'page-popular-products',
  templateUrl: 'popular-products.html',
})
export class PopularProductsPage {

  /**
   * Product List
   */
  productList: any = [];

  constructor(
    public dataProvider: DataProvider,
    public modalCtrl: ModalController) {
  }

  ionViewDidLoad() {
    this.getProducts();
  }

  // Get List of Product
  getProducts() {
    this.dataProvider.getProducts().subscribe((data: any) => {
      this.productList = data;
    });
  }

  openProductDetails(product) {
    const modal = this.modalCtrl.create('ProductDetailsPage', { product: product });
    modal.present();
  }
}

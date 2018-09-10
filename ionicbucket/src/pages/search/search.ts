/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * File path - '../../../src/pages/search/search'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';
import { ProductService } from '../../providers/product/product.service';



@IonicPage()
@Component({
  selector: 'page-search',
  templateUrl: 'search.html',

})
export class SearchPage {

  searchQuery: string = '';
  newProductList: any;
  productList: any;

  constructor(
    public dataProvider: DataProvider,
    public modalCtrl: ModalController,
    private productService: ProductService) {
  }W


  ngOnInit() {
    this.productList = this.productService.products;
    this.newProductList=this.productService.products;
  }

  // Open Product Details Page
  openProductDetails(product) {
    const modal = this.modalCtrl.create('ProductDetailsPage', { product: product });
    modal.present();
  }


  // Search Query
  getItems(ev: any) {
    // Reset items back to all of the items
    this.productList=this.newProductList;    

    // set val to the value of the searchbar
    let val = ev.target.value;

    // if the value is an empty string don't filter the items
    if (val && val.trim() != '') {
      this.productList = this.productList.filter((item) => {
        return (item.productName.toLowerCase().indexOf(val.toLowerCase()) > -1);
      })
    }
  }
}

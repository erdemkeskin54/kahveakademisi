/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Product Component
 * File path - '../../../src/pages/products/products'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';
import { ProductService } from "../../providers/product/product.service";
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { Product } from '../../entities/response-models/product';


@IonicPage()
@Component({
  selector: 'page-products',
  templateUrl: 'products.html',
})
export class ProductsPage {

  productSegment: any;
  productList: Product[];
  apiResponse: ApiResponse;


  constructor(
    public dataProvider: DataProvider,
    public modalCtrl: ModalController,
    private productService: ProductService) {
    this.productSegment = 'list'; // Default Product View

  }

  ionViewDidLoad() {
    this.productList=this.productService.products;
  }




  // Add Product In Cart
  addToCart(product, quantity) {

    // Check If quantity is `undefined` then assign quantity is 1
    if (quantity === undefined) {
      quantity = 1;
    }

    // Merge Quantity with product object
    product = Object.assign(product, { quantity: quantity });

    // Get Storage Data
    let storageData: any = JSON.parse(localStorage.getItem('cartData'));

    // Check is storage data is null or empty,
    // Then add first item in cart and store data in local storage 
    if (storageData === null || storageData === []) {
      storageData = [];
      storageData.push(product);
      localStorage.setItem('cartData', JSON.stringify(storageData));
    }
    //  Update existing storage
    else {
      const isExist = storageData.find(x => x.ID === product.ID);
      if (isExist) {
        storageData = storageData.map((item, index) => {
          if (item.ID === product.ID) {
            storageData[index].quantity = product.quantity;
          }
          return item
        });

        localStorage.setItem('cartData', JSON.stringify(storageData));
      } else {
        storageData.push(product);
        localStorage.setItem('cartData', JSON.stringify(storageData));
      }
    }
  }
}



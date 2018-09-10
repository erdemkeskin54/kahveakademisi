/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Home Component
 * File path - '../../../src/pages/home/home'
 */

import { Component } from '@angular/core';
import { IonicPage, MenuController, ModalController, LoadingController, NavController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';
import { Observable } from 'rxjs/Rx';
import { UserService } from '../../providers/auth/user.service';
import { ProductService } from '../../providers/product/product.service';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { Product } from '../../entities/response-models/product';
import { SocketService } from '../../providers/socket/socket.service';

@IonicPage()
@Component({
  selector: 'page-home',
  templateUrl: 'home.html',
})
export class HomePage {

  productList: Product[];
  apiResponse: ApiResponse;

  newProducts: Observable<any>;
  popularProducts: Observable<any>;

  constructor(public menuCtrl: MenuController,
    public dataProvider: DataProvider,
    public modalCtrl: ModalController,
    private userService: UserService,
    private productService: ProductService,
    private loadingController: LoadingController,
    private nav: NavController,
    private socketService:SocketService) {
    this.menuCtrl.enable(true); // Enable SideMenu
  }

  ionViewDidLoad() {
    this.getProducts();
    console.log(this.userService.getUser());

    if(this.socketService.hubConnection!=null)
    {
      this._setupHubCallbacks();
    }


  }

  getProducts() {
    this.productService.getProducts().subscribe(x => {
      this.apiResponse = x;
      if (this.apiResponse.isSucces == true) {
        this.productList = this.apiResponse.returnObject;
        console.log(this.productList);
      }
      else {
        this.apiResponse.errorContent.forEach(element => {
          console.log(element.errorCode + " : " + element.errorMessage);
        });
      }
    }, (err => {
      console.log(err);
      if (err.status == 401) {
        let loader = this.loadingController.create({
          content: "Yetkisiz giriş. Yönlendiriliyorsunuz.",
          duration: 2000
        });
        loader.present();
        let TIME_IN_MS = 2000;
        setTimeout(() => {
          this.nav.setRoot('SignInPage');
        }, TIME_IN_MS);
      }
    }))
  }

  // Goto Product Details Page
  openProductDetails(product) {
    this.modalCtrl.create('ProductDetailsPage', { product: product }).present();
  }

  _setupHubCallbacks() {
    this.socketService.hubConnection.on('incomingCall', (callingUser) => {

      this.socketService.hubConnection
            .invoke('answerCall', true, callingUser.connectionId)
            .catch(err => console.error(err));

    });

    this.socketService.hubConnection.on('broadCaseMessage', (message) => {

        console.log(message);
    });
}
}

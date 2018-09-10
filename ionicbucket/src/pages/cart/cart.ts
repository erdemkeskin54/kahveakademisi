/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Cart Component
 * File path - '../../../src/pages/cart/cart'
 */

import { Component } from '@angular/core';
import { IonicPage, ModalController, ViewController, AlertController, ToastController } from 'ionic-angular';
import { CartService } from '../../providers/cart/cart-service';

@IonicPage()
@Component({
  selector: 'page-cart',
  templateUrl: 'cart.html',
  providers: [CartService]
})
export class CartPage {

  cartData: any = [];
  total: any = 0;

  constructor(
    public modalCtrl: ModalController,
    public viewCtrl: ViewController,
    public alertCtrl: AlertController,
    private cartService: CartService,
    private toastCtrl:ToastController) {
  }

  ionViewDidLoad() {
    this.getCartData();
  }

  // Get Cart Data
  getCartData() {

    // Get Cart Data From LocalStorage
    this.cartService.getCart().subscribe(x => {
      console.log(x);
      if (x.isSucces) {
        this.cartData = x.returnObject;
        // Calculate Total Value
        this.calculateTotalCart();
      }
    })


  }
  calculateTotalCart() {
    this.total = 0;
    for (var i = 0; i < this.cartData.length; i++) {
      this.total = this.total + this.cartData[i].quantity * this.cartData[i].productAmountType.price;
    }
  }
  // Decrease Quantity
  minusQuantity(item, index) {

    if (this.cartData[index].quantity > 1) {
      this.cartData[index].quantity--;
      this.calculateTotalCart();
      return;
    }

  }

  // Increase Quantity
  addQuantity(item, index) {
    this.cartData[index].quantity++;
    this.calculateTotalCart();
  }

  // Remove Item From Cart
  removeItem(cartId, index) {
    let alert = this.alertCtrl.create({
      title: 'Onaylama',
      message: 'Bu ürünü sepetten silmek istediğinize emin misiniz?',
      buttons: [
        {
          text: 'Vazgec',
          role: 'cancel',
          handler: () => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Evet',
          handler: () => {
            this.cartData.splice(index, 1);
            this.cartService.deleteToCart(cartId).subscribe(x => {
              console.log(x);
              if (x.isSucces) {
                // Calculate Total Value
                this.calculateTotalCart();
              }
            })
          }
        }
      ]
    });
    alert.present();

  }

  updateCart() {

    let updateCart=[];
    this.cartData.forEach(element => {
      updateCart.push({
        "id":element.id,
        "quantity":element.quantity
      });
    });

    let toast = this.toastCtrl.create({
      message: 'Sepetiniz Güncelleniyor',
      position: 'top',
    });


    toast.present().then(()=>{
      this.cartService.updateToCart(updateCart).subscribe(x=>{
        console.log(x);
        toast.setMessage("Sepetniz Güncellendi");

        setTimeout(() => {
          toast.dismiss();
        }, 2000);
     
      });
  
    });
  }
  // Open & Go-To Product Details Page
  openProductDetails(product) {
    this.dismiss(); // Dismiss Current Modal
    this.modalCtrl.create('ProductDetailsPage', { product: product }).present();
  }

  // Open & Go-To Checkout Page
  gotoCheckout() {
    this.dismiss(); // Dismiss Current Modal
    this.modalCtrl.create('CheckoutPage').present();
  }

  /**
   * Dismiss function
   * This function dismiss the popup modal
   */
  dismiss() {
    this.viewCtrl.dismiss();
  }
}

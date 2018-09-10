import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, AlertController, LoadingController, ToastController } from 'ionic-angular';
import { Product } from '../../entities/response-models/product';
import { Cart } from '../../entities/request-models/Cart/cart';
import { CartService } from '../../providers/cart/cart-service';

@IonicPage()
@Component({
  selector: 'page-product-details',
  templateUrl: 'product-details.html',
  providers: [CartService]
})
export class ProductDetailsPage {

  productDetails: Product;
  title: any; // Title Of Page
  carts: any = [];
  cart: any = null;

  productAmountTypes: any = [];



  constructor(private navCtrl: NavController,
    private navParams: NavParams,
    private alertCtrl: AlertController,
    private cartService: CartService,
    private loadingController: LoadingController,
    private nav: NavController,
    private toastCtrl: ToastController) {


  }



  ngOnInit() {


    this.getNavParamsData();

    this.productAmountTypes = [
      {
        checked: false,
        quantity: 1,
        stock: false,
        id: 0
      },
      {
        checked: false,
        quantity: 1,
        stock: false,
        id: 0
      },
      {
        checked: false,
        quantity: 1,
        stock: false,
        id: 0
      },
    ];

    this.productDetails.productAmountTypes.forEach((element, index) => {
      if (element.stock > 0 && element.amountType == 1) {
        this.productAmountTypes[0].checked = true;
        this.productAmountTypes[0].stock = true;
        this.productAmountTypes[0].id = element.id;
      }
      else if (element.stock > 0 && element.amountType == 2) {
        this.productAmountTypes[1].checked = true;
        this.productAmountTypes[1].stock = true;
        this.productAmountTypes[1].id = element.id;
      }
      else if (element.stock > 0 && element.amountType == 3) {
        this.productAmountTypes[2].checked = true;
        this.productAmountTypes[2].stock = true;
        this.productAmountTypes[2].id = element.id;
      }
    });

    var lock = 0;

    this.productAmountTypes.forEach((element, index) => {
      if (element.stock == true && lock == 0) {
        this.productAmountTypes[index].checked = true;
        lock = 1;
      }
      else if (element.stock == true && lock == 1) {
        this.productAmountTypes[index].checked = false;
      }
    });

    console.log(this.productAmountTypes);

  }

  getNavParamsData() {
    if (this.navParams.get('product')) {
      this.productDetails = this.navParams.get('product');
      this.title = this.productDetails.productName;
    }
  }
  stockAlert(value) {
    if (!value) {
      let alert = this.alertCtrl.create({
        title: 'Uyarı',
        subTitle: 'Bu satış tipindeki ürün stokta bulunmuyor',
        buttons: ['Anladım']
      });
      alert.present();
    }
  }
  isChecked1(checked) {
    if (this.productAmountTypes[0].stock == true && checked == true)
      this.productAmountTypes[0].checked = false;
    else if (this.productAmountTypes[0].stock == true && checked == false) {
      this.productAmountTypes[0].checked = true;
    }
    else if (this.productAmountTypes[0].stock == false) {
      this.productAmountTypes[0].checked = false;
    }
  }
  isChecked2(checked) {
    if (this.productAmountTypes[1].stock == true && checked == true)
      this.productAmountTypes[1].checked = false;
    else if (this.productAmountTypes[1].stock == true && checked == false) {
      this.productAmountTypes[1].checked = true;
    }
    else if (this.productAmountTypes[1].stock == false) {
      this.productAmountTypes[1].checked = false;
    }
  }
  isChecked3(checked) {
    if (this.productAmountTypes[2].stock == true && checked == true)
      this.productAmountTypes[2].checked = false;
    else if (this.productAmountTypes[2].stock == true && checked == false) {
      this.productAmountTypes[2].checked = true;
    }
    else if (this.productAmountTypes[2].stock == false) {
      this.productAmountTypes[2].checked = false;
    }
  }

  minusQuantity(index) {

    if (this.productAmountTypes[index].quantity > 1)
      this.productAmountTypes[index].quantity--;

  }

  addQuantity(index) {
    this.productAmountTypes[index].quantity++;
  }

  // Add Product In Cart
  addToCart() {

    let control = true;
    this.productAmountTypes.forEach((element, index) => {
      if (element.checked) {
        this.cart =
          {
            "Id": element.id,
            "quantity": element.quantity
          };
        this.carts.push(this.cart);
      }
    });



    let loader = this.loadingController.create({
      content: "Sepete ekleniyor. Bekleyiniz",
    });
    loader.present().then(() => {

      this.cartService.addToCart(this.carts).subscribe(x => {
        if (x.isSucces) {
          loader.dismiss();
        }
      }, (err => {
        if (err.status == 401) {


          console.log(err);
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

      this.carts = [];
    });


  }
}

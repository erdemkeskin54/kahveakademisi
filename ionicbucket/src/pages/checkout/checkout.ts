
import { Component, ElementRef, ViewChild } from '@angular/core';
import { IonicPage, ViewController, LoadingController, NavController, Nav, AlertController } from 'ionic-angular';
import { CheckoutService } from '../../providers/checkout/checkout.service';
import { AddressService } from '../../providers/address/address.service';
import { InAppBrowser, InAppBrowserOptions } from '@ionic-native/in-app-browser';
import { SocketService } from '../../providers/socket/socket.service';
import { App } from 'ionic-angular';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { DISABLED } from '../../../node_modules/@angular/forms/src/model';


@IonicPage()
@Component({
  selector: 'page-checkout',
  templateUrl: 'checkout.html',
  providers: [CheckoutService, AddressService, InAppBrowser]
})
export class CheckoutPage {


  title: any;
  segment: any;
  model: any = {};
  shownGroup = 1;
  htmlContent: any = null;
  private disabled: boolean = true;
  addresses: any;
  browser: any;
  deneme: boolean;
  creditCard = true;


  addressForm: FormGroup;
  addressTitle: AbstractControl;
  address: AbstractControl;
  city: AbstractControl;
  zip: AbstractControl;
  district: AbstractControl;
  neighborhood: AbstractControl;


  creditCardForm: FormGroup;
  fullname: AbstractControl;
  cardNumber: AbstractControl;
  mounth: AbstractControl;
  year: AbstractControl;
  paymentMethod: AbstractControl;
  installment: AbstractControl;
  cvc: AbstractControl;

  constructor(
    public viewCtrl: ViewController,
    private checkoutService: CheckoutService,
    private elRef: ElementRef,
    private nav: NavController,
    private addressService: AddressService,
    private loadingController: LoadingController,
    private socketService: SocketService,
    private iab: InAppBrowser,
    private app: App,
    private formBuilder: FormBuilder,
    private alertCtrl: AlertController) {

    this.segment = 'address'; // Default Segment
    this.creditCard = true;

    this.addressForm = this.formBuilder.group({
      addressTitle: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      zip: ['', Validators.required],
      district: ['', Validators.required],
      neighborhood: ['', Validators.required]
    });

    this.addressTitle = this.addressForm.controls['addressTitle'];
    this.address = this.addressForm.controls['address'];
    this.city = this.addressForm.controls['city'];
    this.zip = this.addressForm.controls['zip'];
    this.district = this.addressForm.controls['district'];
    this.neighborhood = this.addressForm.controls['neighborhood'];

    this.creditCardForm = this.formBuilder.group({
      fullname: ['', Validators.required],
      cvc: ['', Validators.required],
      mounth: ['', Validators.required],
      year: ['', Validators.required],
      paymentMethod: ['', Validators.required],
      installment: ['', Validators.required],
      cardNumber: ['', Validators.required]
    });

    this.fullname = this.creditCardForm.controls['fullname'];
    this.cvc = this.creditCardForm.controls['cvc'];
    this.mounth = this.creditCardForm.controls['mounth'];
    this.year = this.creditCardForm.controls['year'];
    this.paymentMethod = this.creditCardForm.controls['paymentMethod'];
    this.installment = this.creditCardForm.controls['installment'];
    this.cardNumber = this.creditCardForm.controls['cardNumber'];

    this.paymentMethod.setValue(1);
    this.installment.setValue("1");
    this.fullname.setValue("Mert İĞDİR");
    this.cardNumber.setValue("5528790000000008");
    this.mounth.setValue("12");
    this.year.setValue("30");
    this.cvc.setValue("123");

    this.disabled = true;

  }

  // Next Step Of Segment
  nextstep(segment) {
    this.segment = segment;
  }

  ionViewDidLoad() {
    this.addressService.getAddresses().subscribe(x => {

      if (x.isSucces) {
        this.addresses = x.returnObject;
        if (this.addresses != null) {
          this.model.oldAddressId = this.addresses[0].id;
          this.addressTitle.setValue(this.addresses[0].addressTitle);
          this.address.setValue(this.addresses[0].address);
          this.city.setValue(this.addresses[0].city);
          this.zip.setValue(this.addresses[0].zip);
          this.district.setValue(this.addresses[0].district);
          this.neighborhood.setValue(this.addresses[0].neighborhood);
        }

        console.log(this.addresses);
      }
      else {
        //dönebilecek hataları kontrol et
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
    }));

    if (this.socketService.hubConnection != null) {
      this._setupHubCallbacks();
    }


    this.shownGroup = 1;


  }
  _setupHubCallbacks() {
    this.socketService.hubConnection.on('incomingCall', (callingUser) => {

      this.socketService.hubConnection
        .invoke('answerCall', true, callingUser.connectionId)
        .catch(err => console.error(err));

    });

    this.socketService.hubConnection.on('checkoutNav', () => {
      this.browser.close();
      this.viewCtrl.dismiss().then(() => this.app.getRootNavs()[0].setRoot('MyOrderPage'));
    });
  }

  // Complete Checkout
  finish() {
    if (!this.creditCardForm.invalid) {
      this.model.addressTitle = this.addressTitle.value;
      this.model.address = this.address.value;
      this.model.city = this.city.value;
      this.model.zip = this.zip.value;
      this.model.district = this.district.value;
      this.model.neighborhood = this.neighborhood.value;

      this.model.paymentMethod = this.paymentMethod.value;
      this.model.installment = this.installment.value;
      this.model.fullname = this.fullname.value;
      this.model.cardNumber = this.cardNumber.value;
      this.model.mounth = this.mounth.value;
      this.model.year = this.year.value;
      this.model.cvc = this.cvc.value;
      let errors: any = [];

      this.checkoutService.checkout(this.model).subscribe(x => {

        console.log(x);
        if (x.isSucces) {
          const options: InAppBrowserOptions = {

            location: 'no',
            zoom: 'yes',
            enableViewportScale: 'yes'
          }

          this.htmlContent = x.returnObject;

          var pageContentUrl = "data:text/html;base64," + btoa(this.htmlContent);

          let deneme = '<ion-content><ion-col col-12><iframe src="' + pageContentUrl + '" frameborder="0" style="border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;" allowfullscreen></iframe></ion-col></ion-content>'
          this.browser = this.iab.create('navcheckout.html', '_blank', options);
          this.browser.show();
          this.browser.on("loadstart")
            .subscribe(
              event => {
                console.log("loadstop -->", event);
                if (event.url.indexOf("some error url") > -1) {
                  this.browser.close();
                }
              },
              err => {
                console.log("InAppBrowser loadstart Event Error: " + err);
              });

          this.browser.on("loadstop")
            .subscribe(
              event => {
                //here we call executeScript method of inappbrowser and pass object 
                //with attribute code and value script string which will be executed in the inappbrowser

                this.browser.executeScript({
                  code: "document.write('" + deneme + "')"
                  //code: "document.getElementById('deneme').innetHTML="+deneme+""
                });
                console.log("loadstart -->", event);
              },
              err => {
                console.log("InAppBrowser loadstop Event Error: " + err);
              });
        }
        else {
          x.errorContent.forEach(element => {

            switch (element.errorCode) {
              case "5001": {
                errors.push("<li>" + element.errorMessage + "</li>");
                break;
              }
              case "5003": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              case "6004": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              case "7002": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              case "7004": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              case "8001": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              default:{
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              
            };

          });

          let alert = this.alertCtrl.create({
            title: 'Hata Oluştu',
            subTitle: "<ul>" + errors + "</ul>",
            buttons: ['Kapat']
          });
          alert.present();
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
        else if (err.status == 400) {

          err.error.errorContent.forEach(element => {
            switch (element.errorCode) {
              case "4006": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }
              case "9998": {
                errors.push("<li>"+element.errorMessage+"</li>");
                break;
              }

            };
          });
        }
        else
        {
          errors.push("<li>Beklenmedik bir hata oluştu</li>");
        }

        let alert = this.alertCtrl.create({
          title: 'Hata Oluştu',
          subTitle: "<ul>" + errors + "</ul>",
          buttons: ['Kapat']
        });
        alert.present();
      }));

      
    }

  }

  creditCardControl() {
    if (this.model.paymentMethod == 1) {
      this.creditCard = true;
    }
    else {
      this.creditCard = false;
    }

  }

  addressChange(index) {
    if (this.model.oldAddressId == 0) {

      this.model.oldAddressId = 0;

      this.addressForm.reset();
      this.disabled = false;

    }
    else {

      this.disabled = true;
      this.model.oldAddressId = this.addresses[index].id;
      this.addressTitle.setValue(this.addresses[index].addressTitle);
      this.address.setValue(this.addresses[index].address);
      this.city.setValue(this.addresses[index].city);
      this.zip.setValue(this.addresses[index].zip);
      this.district.setValue(this.addresses[index].district);
      this.neighborhood.setValue(this.addresses[index].neighborhood);


    }
  }

  toggleGroup(group) {
    if (this.isGroupShown(group)) {
      this.shownGroup = null;
    } else {
      this.shownGroup = group;
    }
  };

  isGroupShown(group) {
    return this.shownGroup === group;
  }
}

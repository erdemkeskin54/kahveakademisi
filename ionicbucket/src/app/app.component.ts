
import { Component, ViewChild } from '@angular/core';
import { Nav, Platform, ModalController, NavController } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { TranslateService } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { SocialSharing } from '@ionic-native/social-sharing';
import { Deeplinks } from '@ionic-native/deeplinks';
import { SocketService } from '../providers/socket/socket.service';


@Component({
  templateUrl: 'app.html',
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any = 'SignInPage';


  pages: Array<{ title: string, component: any, leftIcon: string }>;

  constructor(public platform: Platform,
    public statusBar: StatusBar,
    public splashScreen: SplashScreen,
    public http: HttpClient,
    public translateService: TranslateService,
    public modalCtrl: ModalController,
    private socialSharing: SocialSharing,
    private deeplinks: Deeplinks,
    private socketService:SocketService) {



    this.initializeApp();

    translateService.setDefaultLang('tr'); // Default Language
    this.getSideMenuList(); // Get all sidebar menu list
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.statusBar.styleDefault();
      this.splashScreen.hide();
    });
  }





  /**
   * --------------------------------------------------------------
   * Get Sidebar Menu List
   * --------------------------------------------------------------
   */
  getSideMenuList() {
    this.http.get('assets/i18n/tr.json').subscribe((data: any) => {
      this.pages = data.SIDEBAR_MENU;
    }, error => {
      console.error(error);
    });
  }



  /**
   * --------------------------------------------------------------
   * Open Side Menu Pages
   * --------------------------------------------------------------
   */
  openPage(component) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    if (component === 'SearchPage' || component === 'CartPage' || component === 'CheckoutPage') {
      let modal = this.modalCtrl.create(component);
      modal.present();
    } else {
      this.nav.setRoot(component);
    }
  }

  /**
   * --------------------------------------------------------------
   * Rate US Option
   * --------------------------------------------------------------
   */
  rateUs() {
    window.location.href = 'https://play.google.com/store/apps/details?id=com.ionicbucket.multipurposetheme';
  }

  /**
   * --------------------------------------------------------------
   * Share Option
   * --------------------------------------------------------------
   */
  shareSheetShare() {
    const options = {
      message: 'Grocery App',
      subject: 'IonicBucket',
      url: 'https://play.google.com/store/apps/details?id=com.ionicbucket.multipurposetheme'
    }

    this.socialSharing.shareWithOptions(options).then(() => {
      console.log("shareSheetShare: Success");
    }).catch((err) => {
      console.error(err);
    });
  }




}

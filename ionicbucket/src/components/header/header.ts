import { Component, Input } from '@angular/core';
import { ModalController, ViewController } from 'ionic-angular';

@Component({
  selector: 'header',
  templateUrl: 'header.html'
})
export class HeaderComponent {

  @Input('title') title: string; // Page Title

  constructor(public modalCtrl: ModalController,
    public viewCtrl: ViewController) {
  }

  openPage(component) {
    const modal = this.modalCtrl.create(component);
    modal.present();
  }

  /**
   * Dismiss function
   * This function dismiss the popup modal
   */
  dismiss() {
    this.viewCtrl.dismiss();
  }
}

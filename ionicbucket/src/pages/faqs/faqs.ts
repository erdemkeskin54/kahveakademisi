/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent FAQs Component
 * File path - '../../../src/pages/faqs/faqs'
 */

import { Component } from '@angular/core';
import { IonicPage } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';

@IonicPage()
@Component({
  selector: 'page-faqs',
  templateUrl: 'faqs.html',
})
export class FaqsPage {

  /**
   * All FAQs Store in this variable
   */
  allFaqs: any;

  /**
   * Track record of Individual Group variable
   */
  shownGroup = null;

  constructor(
    public dataProvider: DataProvider) {
  }

  ionViewDidLoad() {
    this.getALlFAQs();
  }

  // Get All FAQs
  getALlFAQs() {
    this.dataProvider.getFAQs().subscribe((data) => {
      this.allFaqs = data;
    });
  }

  // Toggle Group
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

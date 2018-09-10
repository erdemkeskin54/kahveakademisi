/**
 * @author    Ionic Bucket <ionicbucket@gmail.com>
 * @copyright Copyright (c) 2018
 * @license   Fulcrumy
 * 
 * This File Represent Categories Component
 * File path - '../../../src/pages/categories/categories'
 */


import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, Platform } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';
import { TranslateService } from '@ngx-translate/core';

@IonicPage()
@Component({
  selector: 'page-categories',
  templateUrl: 'categories.html',
})
export class CategoriesPage {

  /**
   * Selected Category Variable
   */
  category: any;

  /**
   * List of Categories
   */
  categories: any;

  /**
   * List of Sub-Categories
   */
  subCategories: any;

  /**
   * Page Title
   */
  title: any;

  constructor(public navCtrl: NavController,
    public platform: Platform,
    public navParams: NavParams,
    public dataProvider: DataProvider,
    public translate: TranslateService) {

    this.category = this.navParams.get('category'); // Selected Category
    this.title = this.translate.instant('PAGE_TITLE.Categories'); // Page Title

  }

  ionViewDidLoad() {
    this.getCategories();
  }

  // Get All Parent Categories
  getCategories() {
    this.dataProvider.getCategories().subscribe((data) => {
      if (this.category) {
        this.showSubCategory(this.category);
      } else {
        this.categories = data;
      }
    });
  }

  // Show All Sub-Categories of Selected Category
  showSubCategory(category) {
    this.title = category.NAME;
    this.subCategories = category.SUB_CATEGORIES;
  }

  /**
   * Go-To Product Page
   */
  gotoProductPage() {
    this.navCtrl.setRoot('ProductsPage');
  }
}

import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';

@Component({
  selector: 'category-list-view-one',
  templateUrl: 'category-list-view-one.html'
})
export class CategoryListViewOneComponent {

  categories: any;

  constructor(public dataProvider: DataProvider,
    public navCtrl: NavController) {
    this.getCategories();
  }

  getCategories() {
    this.dataProvider.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  gotoCategoryPage(component) {
    this.navCtrl.setRoot(component);
  }

  gotoSubCategoryPage(component, category) {
    this.navCtrl.setRoot(component, { category: category });
  }
}

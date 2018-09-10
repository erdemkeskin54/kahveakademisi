import { Component, Input } from '@angular/core';
import { Product } from '../../entities/response-models/product';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { ModalController } from 'ionic-angular';
import { DataProvider } from '../../providers/data/data';
import { ProductService } from '../../providers/product/product.service';

@Component({
  selector: 'products',
  templateUrl: 'products.html',
  
})
export class ProductsComponent {

  @Input() productList: Product[];


  apiResponse:ApiResponse;

  constructor(private productService:ProductService,  
    private dataProvider: DataProvider,
    private modalCtrl: ModalController) {

  }


  // Open Product Details Page
  openProductDetails(product) {
    const modal = this.modalCtrl.create('ProductDetailsPage', { product: product });
    modal.present();
  }


}

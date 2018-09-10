import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProductsPage } from './products';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    ProductsPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(ProductsPage),
  ],
})
export class ProductsPageModule { }

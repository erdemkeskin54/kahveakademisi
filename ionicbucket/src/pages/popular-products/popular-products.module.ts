import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { PopularProductsPage } from './popular-products';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    PopularProductsPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(PopularProductsPage),
  ],
})
export class PopularProductsPageModule { }

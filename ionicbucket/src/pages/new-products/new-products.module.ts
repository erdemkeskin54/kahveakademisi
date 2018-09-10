import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { NewProductsPage } from './new-products';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    NewProductsPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(NewProductsPage),
  ],
})
export class NewProductsPageModule { }

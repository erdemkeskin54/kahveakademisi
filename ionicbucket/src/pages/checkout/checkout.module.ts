import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CheckoutPage } from './checkout';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';
import { PipesModule } from '../../pipes/pipes.module';

@NgModule({
  declarations: [
    CheckoutPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    PipesModule,
    IonicPageModule.forChild(CheckoutPage),
  ],
})
export class CheckoutPageModule { }

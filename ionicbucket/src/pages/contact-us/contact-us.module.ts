import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ContactUsPage } from './contact-us';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    ContactUsPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(ContactUsPage),
  ],
})
export class ContactUsPageModule { }

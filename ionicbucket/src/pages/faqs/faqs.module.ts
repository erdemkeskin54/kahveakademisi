import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FaqsPage } from './faqs';
import { ComponentsModule } from '../../components/components.module';
import { PipesModule } from '../../pipes/pipes.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    FaqsPage,
  ],
  imports: [
    TranslateModule,
    PipesModule,
    ComponentsModule,
    IonicPageModule.forChild(FaqsPage),
  ],
})
export class FaqsPageModule { }

import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { SearchPage } from './search';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    SearchPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(SearchPage),
  ],
})
export class SearchPageModule { }

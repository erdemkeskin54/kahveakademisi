import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { WeeklySpecialsPage } from './weekly-specials';
import { ComponentsModule } from '../../components/components.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    WeeklySpecialsPage,
  ],
  imports: [
    TranslateModule,
    ComponentsModule,
    IonicPageModule.forChild(WeeklySpecialsPage),
  ],
})
export class WeeklySpecialsPageModule { }

import { NgModule } from '@angular/core';
import { IonicModule } from 'ionic-angular'
import { HomeTopSliderComponent } from './home-top-slider/home-top-slider';
import { HeaderComponent } from './header/header';
import { HeaderOneComponent } from './header-one/header-one';
import { CategoryListViewOneComponent } from './category-list-view-one/category-list-view-one';
import { ProductsComponent } from './products/products';
import { ToFixed } from '../pipes/toFixed/toFixed';
import { TextSplice } from '../pipes/textSplice/textSplice';

@NgModule({
	declarations: [
		HomeTopSliderComponent,
		HeaderComponent,
		HeaderOneComponent,
		CategoryListViewOneComponent,
		ProductsComponent,
		ToFixed,
		TextSplice
	],
	imports: [IonicModule],
	exports: [
		HomeTopSliderComponent,
		HeaderComponent,
		HeaderOneComponent,
		CategoryListViewOneComponent,
		ProductsComponent]
})
export class ComponentsModule { }

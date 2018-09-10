import { NgModule } from '@angular/core';
import { GroupByPipe } from './groupby/groupby';
import { DiscountCalculater } from './discountCalculater/discountCalculater';
import { Sanitize } from './sanitizeHtml/sanitize';

@NgModule({
	declarations: [GroupByPipe,DiscountCalculater,Sanitize],
	imports: [],
	exports: [GroupByPipe,DiscountCalculater,Sanitize]
})
export class PipesModule { }

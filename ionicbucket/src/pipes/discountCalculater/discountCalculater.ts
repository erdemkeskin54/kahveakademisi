import {Pipe} from '@angular/core';
 
@Pipe({
  name: 'discountcalculater'
})
export class DiscountCalculater {
  transform(price:number, discountAmount:number) {

    return price-(price*discountAmount/100);

  }
}
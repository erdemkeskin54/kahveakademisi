import {Pipe} from '@angular/core';
 
@Pipe({
  name: 'tofixed'
})
export class ToFixed {
  transform(value:number, count:number) {

    return value.toFixed(count);
  }
}
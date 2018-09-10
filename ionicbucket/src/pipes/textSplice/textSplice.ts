import {Pipe} from '@angular/core';
 
@Pipe({
  name: 'textsplice'
})
export class TextSplice {
  transform(value:string, count:number) {

    if(value.length>count)
         return value.slice(0,count);
    
    return value;
  }
}
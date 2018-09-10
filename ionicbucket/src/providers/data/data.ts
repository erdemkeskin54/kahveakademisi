import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class DataProvider {

  constructor(public http: HttpClient) {
    console.log('Hello DataProvider Provider');
  }

  getCategories() {
    return Observable.create(observer => {
      this.http.get('assets/i18n/tr.json').subscribe((result: any) => {
        observer.next(result.CATEGORIES);
        observer.complete();
      });
    });
  }

  getProducts() {
    return Observable.create(observer => {
      this.http.get('assets/i18n/tr.json').subscribe((result: any) => {
        observer.next(result.PRODUCTS);
        observer.complete();
      });
    });
  }

  getFAQs() {
    return Observable.create(observer => {
      this.http.get('assets/i18n/tr.json').subscribe((result: any) => {
        observer.next(result.FAQS);
        observer.complete();
      });
    });
  }
}

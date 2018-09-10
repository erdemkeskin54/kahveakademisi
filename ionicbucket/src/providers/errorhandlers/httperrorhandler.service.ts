import { ErrorHandler, Injectable, ViewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable } from '../../../node_modules/rxjs';
import { NavController,  LoadingController, Nav } from 'ionic-angular';

@Injectable()
export class HttperrorhandlerService implements ErrorHandler {

  @ViewChild(Nav) nav:Nav;

  constructor(
    public loadingController: LoadingController) { 

    }

  handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      if (error.status === 500) {
        return Observable.throw(new Error(error.error));
      }
      else if (error.status === 400) {
        return Observable.throw(new Error(error.error));
      }
      else if (error.status === 409) {
        return Observable.throw(new Error(error.error));
      }
      else if (error.status === 406) {
        return Observable.throw(new Error(error.error));
      }
      else if (error.status === 401) {

        let loader = this.loadingController.create({
          content: "Yetkisiz giriş. Yönlendiriliyorsunuz.",
          duration: 2000
        })
        loader.present();
        let TIME_IN_MS = 2000;

        setTimeout(() => {
          this.nav.setRoot('SignInPage');
        }, TIME_IN_MS);
        

        console.log("helllllooo");
        return Observable.throw(error);
      }
      console.log(error);
      console.error(`Backend returned code ${error.status}, `);
      error.error.errorContent.forEach(element => {
        console.log(element);
      });
    }
    // return an observable with a user-facing error message
    return Observable.throw(error.error);
  };
}

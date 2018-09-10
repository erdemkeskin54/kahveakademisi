import { Injectable, Inject, ViewChild, DoCheck } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";

import { Observable } from 'rxjs';

import { catchError, retry, map } from 'rxjs/operators';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { UserService } from '../auth/user.service';


@Injectable()
export class CheckoutService{


    constructor(private http: HttpClient,
        private userService: UserService,
        @Inject("apiUrl") private apiUrl,
    ) {

    }

    checkout(data): Observable<ApiResponse> {

        let header: HttpHeaders = new HttpHeaders(
            { 'Authorization': 'Bearer ' + this.userService.getUser().token }
        );

        console.log(this.userService.getUser().token);

        return this.http.post(this.apiUrl+'/checkout/checkout/?culture=tr-TR',data ,{ headers: header })
            .pipe(
                map(data =>data),
                retry(0),
                catchError((error: HttpErrorResponse) => {
                    console.log(error);
                    return Observable.throw(error);
                })
            );

    }

    getCart(): Observable<ApiResponse> {

        let header: HttpHeaders = new HttpHeaders(
            { 'Authorization': 'Bearer ' + this.userService.getUser().token }
        );
        return this.http.get(this.apiUrl + '/cart/getcart/?culture=tr-TR', { headers: header })
            .pipe(
                map(data => {
                    return data;
                }),
                retry(3),
                catchError((error: HttpErrorResponse) => {
                    return Observable.throw(error);
                })
            );
    }
}


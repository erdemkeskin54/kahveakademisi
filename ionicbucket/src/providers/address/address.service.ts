import { Injectable, Inject, ViewChild, DoCheck } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";

import { Observable } from 'rxjs';

import { catchError, retry, map } from 'rxjs/operators';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { UserService } from '../auth/user.service';


@Injectable()
export class AddressService{


    constructor(private http: HttpClient,
        private userService: UserService,
        @Inject("apiUrl") private apiUrl,
    ) {

    }


    getAddresses(): Observable<ApiResponse> {

        let header: HttpHeaders = new HttpHeaders(
            { 'Authorization': 'Bearer ' + this.userService.getUser().token }
        );
        return this.http.get(this.apiUrl + '/addresses/getuseraddresses/?culture=tr-TR', { headers: header })
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


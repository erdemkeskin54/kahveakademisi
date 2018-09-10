import { Injectable, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";

import { Observable } from 'rxjs';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { catchError, retry, map } from 'rxjs/operators';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { UserService } from '../auth/user.service';
import { Product } from '../../entities/response-models/product';


@Injectable()
export class ProductService {

    public products: Product[];

    constructor(private http: HttpClient,
        private userService: UserService,
        @Inject("apiUrl") private apiUrl,
    ) {
    }

    getProducts(): Observable<ApiResponse> {
        
        let header: HttpHeaders = new HttpHeaders(
            { 'Authorization': 'Bearer ' + this.userService.getUser().token }
        );

        return this.http.get(this.apiUrl + '/products/getallproducts/?culture=tr-TR', { headers: header })
            .pipe(
                map(data => {
                    this.products = data["returnObject"];
                    return data;
                }),
                retry(3),
                catchError((error: HttpErrorResponse) => {
                    return Observable.throw(error);
                })
            );
    }
}


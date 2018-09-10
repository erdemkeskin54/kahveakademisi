import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';

import { Observable } from 'rxjs';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ApiResponse } from '../../entities/infrastructure/api-response';
import { Login } from '../../entities/request-models/Auth/login';



@Injectable()
export class AuthService {

    token:string;


    constructor(private http: Http,
        @Inject("apiUrl") private apiUrl) {

    }

    login(login: Login): Observable<ApiResponse> {
        return this.http.post(this.apiUrl + '/auth/login/?culture=tr-TR', login)
            .map(response => response.json());
    }



}
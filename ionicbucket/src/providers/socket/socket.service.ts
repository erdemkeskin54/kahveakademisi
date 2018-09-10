import { Injectable, Inject, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";

import { Observable } from 'rxjs';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';;
import { UserService } from '../auth/user.service';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { NavController } from '../../../node_modules/ionic-angular/umd';


@Injectable()
export class SocketService {
    public hubConnection: HubConnection;

    constructor(private http: HttpClient,
        private userService: UserService,
        @Inject("apiUrl") private apiUrl
    ) {
    }


    _connect():any{
        // Set Up SignalR Signaler
        this.hubConnection = new HubConnectionBuilder().withUrl("http://192.168.1.100:2176/hub").build();

        this.hubConnection
            .start()
            .then((data) => {
                console.log('Connection started!')
                this.hubConnection
                    .invoke('deneme')
                    .catch(err => console.error(err));

                this._setupHubCallbacks();

                return true;
            })
            .catch(err => console.log('Error while establishing connection :('));

        // Setup client SignalR operations

    }

    _setupHubCallbacks() {
        this.hubConnection.on('incomingCall', (callingUser) => {

            this.hubConnection
                .invoke('answerCall', true, callingUser.connectionId)
                .catch(err => console.error(err));

        });

        this.hubConnection.on('broadCaseMessage', (message) => {

            alert(message+"123");
        });
    }
}


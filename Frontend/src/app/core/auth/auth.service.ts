import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest, LoginResponse, RegisterRequest } from './auth.model';
import { Observable } from 'rxjs';
import { ApiResponse } from '@/app/core/http/api-response.model';
import { API_URLS } from '@/app/core/config';

@Injectable( { providedIn: 'root' } )
export class AuthService {
    constructor (
        private http: HttpClient,
    ) { }

    login ( payload: LoginRequest ): Observable<ApiResponse<LoginResponse>> {
        return this.http.post<ApiResponse<LoginResponse>>( API_URLS.auth.login(), payload );
    }

    register ( payload: RegisterRequest ): Observable<ApiResponse<LoginResponse>> {
        return this.http.post<ApiResponse<LoginResponse>>( API_URLS.auth.register(), payload );
    }

    logout (): Observable<void> {
        return this.http.post<void>( API_URLS.auth.logout(), {} );
    }
}
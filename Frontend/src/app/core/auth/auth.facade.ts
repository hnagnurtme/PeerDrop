import { Injectable, signal } from "@angular/core";
import { LoginRequest, RegisterRequest, User } from "./auth.model";
import { AuthService } from "./auth.service";
import { StorageService } from "@/app/core/services/storage.service";
import { tap } from "rxjs";

@Injectable( { providedIn: 'root' } )
export class AuthFacade {
    private readonly _user = signal<User | null>( null );
    private readonly _isAuthenticated = signal<boolean>( false );
    private readonly _isLoading = signal<boolean>( false );

    public readonly user = this._user.asReadonly();
    public readonly isAuthenticated = this._isAuthenticated.asReadonly();
    public readonly isLoading = this._isLoading.asReadonly();

    constructor (
        private authService: AuthService,
        private storageService: StorageService
    ) {
        this.initFromStorage();
    }

    private initFromStorage (): void {
        const token = this.storageService.getAccessToken();
        const user = this.storageService.getUser<User>();
        if ( token && user ) {
            this._user.set( user );
            this._isAuthenticated.set( true );
        }
    }

    login ( payload: LoginRequest ) {
        this._isLoading.set( true );
        return this.authService.login( payload ).pipe(
            tap( response => {
                this._isLoading.set( false );
                if ( !response.isSuccess ) return;

                const { accessToken, refreshToken, user } = response.data;
                this.storageService.setAccessToken( accessToken );
                this.storageService.setRefreshToken( refreshToken );
                this.storageService.setUser( user );
                this._user.set( user );
                this._isAuthenticated.set( true );
            } )
        );
    }

    register ( payload: RegisterRequest ) {
        this._isLoading.set( true );
        return this.authService.register( payload ).pipe(
            tap( response => {
                this._isLoading.set( false );
                if ( !response.isSuccess ) return;

                const { accessToken, refreshToken, user } = response.data;
                this.storageService.setAccessToken( accessToken );
                this.storageService.setRefreshToken( refreshToken );
                this.storageService.setUser( user );
                this._user.set( user );
                this._isAuthenticated.set( true );
            } )
        );
    }

    logout () {
        return this.authService.logout().pipe(
            tap( () => {
                this.storageService.clearTokens();
                this._user.set( null );
                this._isAuthenticated.set( false );
            } )
        );
    }
}
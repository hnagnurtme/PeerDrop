import { Injectable } from "@angular/core";

const STORAGE_KEYS = {
    ACCESS_TOKEN: 'peerdrop_access_token',
    REFRESH_TOKEN: 'peerdrop_refresh_token',
    USER: 'peerdrop_user'
} as const;

@Injectable( { providedIn: 'root' } )
export class StorageService {
    setAccessToken ( token: string ): void {
        localStorage.setItem( STORAGE_KEYS.ACCESS_TOKEN, token );
    }

    getAccessToken (): string | null {
        return localStorage.getItem( STORAGE_KEYS.ACCESS_TOKEN );
    }

    setRefreshToken ( token: string ): void {
        localStorage.setItem( STORAGE_KEYS.REFRESH_TOKEN, token );
    }

    getRefreshToken (): string | null {
        return localStorage.getItem( STORAGE_KEYS.REFRESH_TOKEN );
    }

    clearTokens (): void {
        localStorage.removeItem( STORAGE_KEYS.ACCESS_TOKEN );
        localStorage.removeItem( STORAGE_KEYS.REFRESH_TOKEN );
        localStorage.removeItem( STORAGE_KEYS.USER );
    }

    setUser ( user: object ): void {
        localStorage.setItem( STORAGE_KEYS.USER, JSON.stringify( user ) );
    }

    getUser<T> (): T | null {
        const data = localStorage.getItem( STORAGE_KEYS.USER );
        return data ? JSON.parse( data ) : null;
    }
}
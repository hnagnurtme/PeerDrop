import { Injectable } from "@angular/core";

const STORAGE_KEYS = {
    ACCESS_TOKEN: 'peerdrop_access_token',
    REFRESH_TOKEN: 'peerdrop_refresh_token',
    USER: 'peerdrop_user',
    REMEMBERED_LOGIN: 'peerdrop_remembered_login'
} as const;

const REMEMBER_ME_EXPIRY = 30 * 24 * 60 * 60 * 1000; 

export interface RememberedCredentials {
    email: string;
    timestamp: number;
}

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

    setRememberedLogin ( email: string ): void {
        const data: RememberedCredentials = {
            email,
            timestamp: Date.now()
        };
        localStorage.setItem( STORAGE_KEYS.REMEMBERED_LOGIN, JSON.stringify( data ) );
    }

    getRememberedLogin (): RememberedCredentials | null {
        try {
            const data = localStorage.getItem( STORAGE_KEYS.REMEMBERED_LOGIN );
            if ( !data ) return null;

            const parsed: RememberedCredentials = JSON.parse( data );
            const isExpired = Date.now() - parsed.timestamp > REMEMBER_ME_EXPIRY;

            if ( isExpired ) {
                this.removeRememberedLogin();
                return null;
            }

            return parsed;
        } catch {
            this.removeRememberedLogin();
            return null;
        }
    }

    removeRememberedLogin (): void {
        localStorage.removeItem( STORAGE_KEYS.REMEMBERED_LOGIN );
    }
}
import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})
export class StorageService {
    setAccessToken(token: string): void {
    }

    getAccessToken(): string | null {
        return null;
    }

    setRefreshToken(token: string): void {
    }

    getRefreshToken(): string | null {
        return null;
    }

    clearTokens(): void {
    }
}
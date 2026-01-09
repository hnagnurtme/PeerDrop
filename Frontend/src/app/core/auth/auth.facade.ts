import { Injectable, signal } from "@angular/core";
import { LoginRequest, User } from "./auth.model";
import { AuthService } from "./auth.service";
import { StorageService } from "@/app/core/services/storage.service";
import { tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AuthFacade {
    private readonly user  = signal<User | null>(null);

    public readonly isAuthenticated = signal<boolean>(false);

    constructor(
        private authService : AuthService,
        private storageService : StorageService
    ) {}

    login(payload : LoginRequest){
        return this.authService.login(payload).pipe(
            tap(response => {
                    if(!response.isSuccess) return;

                    const { accessToken, refreshToken, user } = response.data;

                    this.storageService.setAccessToken(accessToken);
                    this.storageService.setRefreshToken(refreshToken);
                    this.user.set(user);
                    this.isAuthenticated.set(true);
            })
        );
    }


    logout(){
        return this.authService.logout().pipe(
            tap(() => {
                this.storageService.clearTokens();
                this.user.set(null);
                this.isAuthenticated.set(false);
            })  
        )
    }
}
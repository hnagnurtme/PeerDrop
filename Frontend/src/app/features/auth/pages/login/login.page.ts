import { Component } from '@angular/core';
import { AuthGlassCardComponent } from '@/app/features/auth/components/auth-glass-card/auth-glass-card.component';
import { AuthHeroComponent } from '@/app/features/auth/components/auth-hero/auth-hero.component';
import { AuthFeatureGridComponent } from '@/app/features/auth/components/auth-feature-grid/auth-feature-grid.component';
import { LoginLayoutComponent } from '@/app/features/auth/layouts/login-layout/login-layout.component';
import { LogoComponent } from "@/app/shared/components/logo/logo.component";

@Component( {
    selector: 'app-login',
    imports: [
    LogoComponent,
    AuthGlassCardComponent,
    AuthHeroComponent,
    AuthFeatureGridComponent,
    LoginLayoutComponent,
],
    templateUrl: './login.page.html',
    styleUrls: [ './login.page.scss' ]
} )
export class LoginComponent { }

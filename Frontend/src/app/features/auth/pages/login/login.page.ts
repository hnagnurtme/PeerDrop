import { Component } from '@angular/core';
import { AuthLogoComponent } from '@/app/features/auth/components/auth-logo/auth-logo.component';
import { AuthGlassCardComponent } from '@/app/features/auth/components/auth-glass-card/auth-glass-card.component';
import { AuthHeroComponent } from '@/app/features/auth/components/auth-hero/auth-hero.component';
import { AuthFeatureGridComponent } from '@/app/features/auth/components/auth-feature-grid/auth-feature-grid.component';
import { AuthSecurityBadgeComponent } from '@/app/features/auth/components/auth-security-badge/auth-security-badge.component';
import { LoginLayoutComponent } from '@/app/features/auth/layouts/login-layout/login-layout.component';

@Component( {
    selector: 'app-login',
    imports: [
        AuthLogoComponent,
        AuthGlassCardComponent,
        AuthHeroComponent,
        AuthFeatureGridComponent,
        AuthSecurityBadgeComponent,
        LoginLayoutComponent
    ],
    templateUrl: './login.page.html',
    styleUrls: [ './login.page.scss' ]
} )
export class LoginComponent { }

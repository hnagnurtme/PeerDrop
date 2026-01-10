import { Component } from '@angular/core';
import { AuthLogoComponent } from '../../components/auth-logo/auth-logo.component';
import { AuthGlassCardComponent } from '../../components/auth-glass-card/auth-glass-card.component';
import { AuthHeroComponent } from '../../components/auth-hero/auth-hero.component';
import { AuthFeatureGridComponent } from '../../components/auth-feature-grid/auth-feature-grid.component';
import { AuthSecurityBadgeComponent } from '../../components/auth-security-badge/auth-security-badge.component';
import { LoginLayoutComponent } from '../../layouts/login-layout/login-layout.component';

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

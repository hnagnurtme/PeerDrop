import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthSsoButtonComponent } from '@/app/features/auth/components/auth-sso-button/auth-sso-button.component';
import { LoginFormComponent } from '@/app/features/auth/forms/login-form/login-form.component';
import { AuthFacade, LoginRequest } from '@/app/core/auth';
import { Router } from '@angular/router';

@Component( {
    selector: 'app-login-layout',
    standalone: true,
    imports: [
        CommonModule,
        RouterLink,
        AuthSsoButtonComponent,
        LoginFormComponent
    ],
    templateUrl: './login-layout.component.html',
    styleUrls: [ './login-layout.component.scss' ]
} )
export class LoginLayoutComponent {
    private authFacade = inject( AuthFacade );
    private router = inject( Router );

    readonly isLoading = this.authFacade.isLoading;

    onFormSubmit ( credentials: LoginRequest ): void {
        this.authFacade.login( credentials ).subscribe( {
            next: ( response ) => {
                if ( response.isSuccess ) {
                    this.router.navigate( [ '/' ] );
                }
            },
            error: ( err ) => {
                console.error( 'Login failed:', err );
            }
        } );
    }
}

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthSsoButtonComponent } from '@/app/features/auth/components/auth-sso-button/auth-sso-button.component';
import { LoginFormComponent } from '@/app/features/auth/forms/login-form/login-form.component';
import { AuthFacade, LoginRequest } from '@/app/core/auth';
import { ToastService, getAuthErrorMessage } from '@/app/shared';

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
    private toast = inject( ToastService );

    readonly isLoading = this.authFacade.isLoading;

    onFormSubmit ( credentials: LoginRequest ): void {
        this.authFacade.login( credentials ).subscribe( {
            next: ( response ) => {
                if ( response.isSuccess ) {
                    this.toast.success( 'Login successful! Redirecting...' );
                    this.router.navigate( [ '/' ] );
                } else {
                    this.toast.error( response.message || 'Login failed. Please try again.' );
                }
            },
            error: ( err: HttpErrorResponse ) => {
                this.toast.error( getAuthErrorMessage( err ) );
            }
        } );
    }
}

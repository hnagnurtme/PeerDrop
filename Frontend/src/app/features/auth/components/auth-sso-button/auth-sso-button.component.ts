import { Component, Input } from '@angular/core';

@Component( {
    selector: 'auth-sso-button',
    standalone: true,
    templateUrl: './auth-sso-button.component.html'
} )
export class AuthSsoButtonComponent {
    @Input() text = 'Continue with Google';
}

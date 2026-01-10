import { Component, Input } from '@angular/core';

@Component( {
    selector: 'auth-sso-button',
    standalone: true,
    templateUrl: './auth-sso-button.component.html',
    styleUrls: [ './auth-sso-button.component.scss' ],
} )
export class AuthSsoButtonComponent {
    @Input() text = 'Continue with Google';
}

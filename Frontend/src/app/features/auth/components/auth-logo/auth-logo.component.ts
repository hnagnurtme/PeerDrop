import { Component, Input } from '@angular/core';

@Component( {
    selector: 'auth-logo',
    standalone: true,
    templateUrl: './auth-logo.component.html',
    styleUrls: [ './auth-logo.component.scss' ],
} )
export class AuthLogoComponent {
    @Input() title = 'PeerDrop';
}

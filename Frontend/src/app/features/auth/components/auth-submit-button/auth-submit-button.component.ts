import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component( {
    selector: 'auth-submit-button',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './auth-submit-button.component.html',
    styleUrls: [ './auth-submit-button.component.scss' ],
} )
export class AuthSubmitButtonComponent {
    @Input() text = 'Submit';
    @Input() icon?: string;
}

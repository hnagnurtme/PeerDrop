import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component( {
    selector: 'auth-submit-button',
    standalone: true,
    imports: [ CommonModule ],
    templateUrl: './auth-submit-button.component.html'
} )
export class AuthSubmitButtonComponent {
    @Input() text = 'Submit';
    @Input() icon?: string;
    @Input() disabled = false;
    @Input() isLoading = false;
}

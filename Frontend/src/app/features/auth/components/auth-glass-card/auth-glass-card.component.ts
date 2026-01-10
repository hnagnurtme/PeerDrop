import { Component } from '@angular/core';

@Component( {
    selector: 'auth-glass-card',
    standalone: true,
    templateUrl: './auth-glass-card.component.html',
    styleUrls: [ './auth-glass-card.component.scss' ],
    host: {
        'class': 'w-full max-w-[520px]'
    }
} )
export class AuthGlassCardComponent { }

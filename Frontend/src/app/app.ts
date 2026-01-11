import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastContainerComponent } from '@/app/shared';

@Component( {
    selector: 'app-root',
    imports: [ RouterOutlet, ToastContainerComponent ],
    templateUrl: './app.html',
    styleUrl: './app.scss'
} )
export class App {
    protected readonly title = signal( 'Frontend' );
}

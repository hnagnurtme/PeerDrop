import { Injectable, signal } from '@angular/core';

export type ToastType = 'success' | 'error' | 'warning' | 'info';

export interface Toast {
    id: string;
    message: string;
    type: ToastType;
    duration?: number;
}

const DEFAULT_DURATION = 5000;

@Injectable( { providedIn: 'root' } )
export class ToastService {
    private readonly _toasts = signal<Toast[]>( [] );
    readonly toasts = this._toasts.asReadonly();

    show ( message: string, type: ToastType = 'info', duration = DEFAULT_DURATION ): string {
        const id = this.generateId();
        const toast: Toast = { id, message, type, duration };

        this._toasts.update( toasts => [ ...toasts, toast ] );

        if ( duration > 0 ) {
            setTimeout( () => this.dismiss( id ), duration );
        }

        return id;
    }

    success ( message: string, duration = DEFAULT_DURATION ): string {
        return this.show( message, 'success', duration );
    }

    error ( message: string, duration = DEFAULT_DURATION ): string {
        return this.show( message, 'error', duration );
    }

    warning ( message: string, duration = DEFAULT_DURATION ): string {
        return this.show( message, 'warning', duration );
    }

    info ( message: string, duration = DEFAULT_DURATION ): string {
        return this.show( message, 'info', duration );
    }

    dismiss ( id: string ): void {
        this._toasts.update( toasts => toasts.filter( t => t.id !== id ) );
    }

    dismissAll (): void {
        this._toasts.set( [] );
    }

    private generateId (): string {
        return `toast-${ Date.now() }-${ Math.random().toString( 36 ).substring( 2, 9 ) }`;
    }
}

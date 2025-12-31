import { useEffect, useState } from 'react';

export interface Toast {
    id: string;
    message: string;
    type: 'error' | 'success' | 'warning' | 'info';
    duration?: number;
}

class ToastManager {
    private listeners: Set<( toasts: Toast[] ) => void> = new Set();
    private toasts: Toast[] = [];

    subscribe ( listener: ( toasts: Toast[] ) => void ) {
        this.listeners.add( listener );
        return () => {
            this.listeners.delete( listener );
        };
    }

    private notify () {
        this.listeners.forEach( listener => listener( this.toasts ) );
    }

    show ( message: string, type: Toast[ 'type' ] = 'info', duration = 5000 ) {
        const id = Math.random().toString( 36 ).substring( 7 );
        const toast: Toast = { id, message, type, duration };

        this.toasts = [ ...this.toasts, toast ];
        this.notify();

        if ( duration > 0 ) {
            setTimeout( () => this.dismiss( id ), duration );
        }

        return id;
    }

    dismiss ( id: string ) {
        this.toasts = this.toasts.filter( t => t.id !== id );
        this.notify();
    }

    error ( message: string, duration?: number ) {
        return this.show( message, 'error', duration );
    }

    success ( message: string, duration?: number ) {
        return this.show( message, 'success', duration );
    }

    warning ( message: string, duration?: number ) {
        return this.show( message, 'warning', duration );
    }

    info ( message: string, duration?: number ) {
        return this.show( message, 'info', duration );
    }
}

export const toast = new ToastManager();

export function useToasts () {
    const [ toasts, setToasts ] = useState<Toast[]>( [] );

    useEffect( () => {
        const unsubscribe = toast.subscribe( setToasts );
        return unsubscribe;
    }, [] );

    return toasts;
}

import { Component, inject } from '@angular/core';
import { ToastService } from '../../services/toast.service';

@Component( {
    selector: 'app-toast-container',
    standalone: true,
    templateUrl: './toast-container.component.html',
    styles: [ `
        @keyframes slide-in {
            from {
                opacity: 0;
                transform: translateX(100%);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        .animate-slide-in {
            animation: slide-in 0.3s ease-out forwards;
        }
    `]
} )
export class ToastContainerComponent {
    protected toastService = inject( ToastService );

    protected getToastClasses ( type: string ): string {
        const base = 'transition-all duration-300';
        const variants: Record<string, string> = {
            success: 'bg-emerald-500/90 border-emerald-400/50 text-white',
            error: 'bg-red-500/90 border-red-400/50 text-white',
            warning: 'bg-amber-500/90 border-amber-400/50 text-white',
            info: 'bg-blue-500/90 border-blue-400/50 text-white'
        };
        return `${ base } ${ variants[ type ] || variants[ 'info' ] }`;
    }

    protected getIcon ( type: string ): string {
        const icons: Record<string, string> = {
            success: 'check_circle',
            error: 'error_outline',
            warning: 'warning_amber',
            info: 'info'
        };
        return icons[ type ] || icons[ 'info' ];
    }

    protected dismiss ( id: string ): void {
        this.toastService.dismiss( id );
    }
}

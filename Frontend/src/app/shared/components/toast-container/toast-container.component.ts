import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component( {
    selector: 'app-toast-container',
    standalone: true,
    imports: [ CommonModule ],
    templateUrl: './toast-container.component.html',
    styles: [ `
        @keyframes slide-in {
            from {
                opacity: 0;
                transform: translateX(100%) scale(0.95);
            }
            to {
                opacity: 1;
                transform: translateX(0) scale(1);
            }
        }

        .animate-slide-in {
            animation: slide-in 0.25s ease-out forwards;
        }
    `]
} )
export class ToastContainerComponent {
    protected toastService = inject( ToastService );

    protected getToastClasses ( type: string ): string {
        const variants: Record<string, string> = {
            success: 'bg-toast-success/95 border-toast-success/40 text-white',
            error: 'bg-toast-error/95 border-toast-error/40 text-white',
            warning: 'bg-toast-warning/95 border-toast-warning/40 text-white',
            info: 'bg-toast-info/95 border-toast-info/40 text-white'
        };
        return variants[ type ] || variants[ 'info' ];
    }

    protected getIcon ( type: string ): string {
        const icons: Record<string, string> = {
            success: 'check_circle',
            error: 'error',
            warning: 'warning',
            info: 'info'
        };
        return icons[ type ] || icons[ 'info' ];
    }

    protected dismiss ( id: string ): void {
        this.toastService.dismiss( id );
    }
}

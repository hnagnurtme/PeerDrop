import { useToasts, toast as toastManager,type Toast } from './toastManager';
import { Icons } from '@/shared/constants/Icon';

export default function Toaster () {
    const toasts = useToasts();

    const getToastStyles = ( type: Toast[ 'type' ] ) => {
        switch ( type ) {
            case 'error':
                return 'bg-red-500 border-black';
            case 'success':
                return 'bg-green-500 border-black';
            case 'warning':
                return 'bg-yellow-500 border-black';
            case 'info':
            default:
                return 'bg-blue-500 border-black';
        }
    };

    const getIcon = ( type: Toast[ 'type' ] ) => {
        switch ( type ) {
            case 'error':
                return '⚠️';
            case 'success':
                return '✓';
            case 'warning':
                return '⚡';
            case 'info':
            default:
                return 'ℹ';
        }
    };

    if ( toasts.length === 0 ) return null;

    return (
        <div className="fixed bottom-6 right-6 z-50 flex flex-col gap-3 max-w-md">
            { toasts.map( ( toast, index ) => (
                <div
                    key={ toast.id }
                    className={ `
                        ${ getToastStyles( toast.type ) }
                        text-white px-6 py-4 border-4 shadow-brutal
                        font-mono text-sm
                        transform transition-all duration-300 ease-out
                        animate-slide-in-right
                    ` }
                    style={ {
                        animationDelay: `${ index * 100 }ms`,
                    } }
                >
                    <div className="flex items-start gap-3">
                        <span className="text-xl flex-shrink-0">{ getIcon( toast.type ) }</span>
                        <div className="flex-1">
                            <p className="break-words">{ toast.message }</p>
                        </div>
                        <button
                            onClick={ () => toastManager.dismiss( toast.id ) }
                            className="flex-shrink-0 hover:scale-110 transition-transform"
                            aria-label="Close"
                        >
                            <Icons.GridView />
                        </button>
                    </div>
                </div>
            ) ) }
        </div>
    );
}

export { toast } from './toastManager';

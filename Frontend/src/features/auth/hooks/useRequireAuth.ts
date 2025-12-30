import { useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../store';

interface UseRequireAuthOptions {
    redirectTo?: string;
}

export function useRequireAuth ( options: UseRequireAuthOptions = {} ) {
    const { redirectTo = '/login' } = options;
    const navigate = useNavigate();
    const location = useLocation();
    const { isAuthenticated, isLoading } = useAuthStore();

    useEffect( () => {
        if ( !isLoading && !isAuthenticated ) {
            navigate( redirectTo, {
                replace: true,
                state: { from: location.pathname }
            } );
        }
    }, [ isAuthenticated, isLoading, navigate, redirectTo, location.pathname ] );

    return { isAuthenticated, isLoading };
}

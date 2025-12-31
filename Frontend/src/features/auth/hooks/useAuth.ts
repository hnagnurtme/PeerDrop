import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store';
import { authService } from '../services';
import type { LoginFormData, SignupFormData } from '../schemas';

export function useAuth () {
    const navigate = useNavigate();
    const {
        user,
        isAuthenticated,
        isLoading,
        error,
        login: storeLogin,
        logout: storeLogout,
        setLoading,
        setError,
        clearError
    } = useAuthStore();

    const login = useCallback( async ( data: LoginFormData ) => {
        try {
            setLoading( true );
            clearError();

            const response = await authService.login( data );
            storeLogin( response.user, response.accessToken );
            navigate( '/' );

            return response;
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Login failed';
            setError( message );
            throw err;
        } finally {
            setLoading( false );
        }
    }, [ navigate, storeLogin, setLoading, setError, clearError ] );

    const signup = useCallback( async ( data: SignupFormData ) => {
        try {
            setLoading( true );
            clearError();

            const { confirmPassword, ...signupData } = data;
            const response = await authService.signup( signupData );
            storeLogin( response.user, response.accessToken );
            navigate( '/' );

            return response;
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Signup failed';
            setError( message );
            throw err;
        } finally {
            setLoading( false );
        }
    }, [ navigate, storeLogin, setLoading, setError, clearError ] );

    const logout = useCallback( async () => {
        try {
            setLoading( true );
            await authService.logout();
        } catch {
        } finally {
            storeLogout();
            setLoading( false );
            navigate( '/login' );
        }
    }, [ navigate, storeLogout, setLoading ] );

    return {
        user,
        isAuthenticated,
        isLoading,
        error,
        login,
        signup,
        logout,
        clearError,
    };
}

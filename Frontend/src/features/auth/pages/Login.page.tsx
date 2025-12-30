import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import LoginForm from '../components/LoginForm';
import { authService } from '../services';
import { useAuthStore } from '../store';

export default function LoginPage () {
    const navigate = useNavigate();
    const { login, setLoading, setError } = useAuthStore();
    const [ errorMessage, setErrorMessage ] = useState<string | null>( null );

    const handleLogin = async ( email: string, password: string ) => {
        try {
            setLoading( true );
            setErrorMessage( null );

            const response = await authService.login( { email, password } );
            login( response.user, response.token );
            navigate( '/' );
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Login failed';
            setErrorMessage( message );
            setError( message );
        } finally {
            setLoading( false );
        }
    };

    const handleGoToSignup = () => {
        navigate( '/signup' );
    };

    return (
        <div>
            { errorMessage && (
                <div className="fixed top-20 left-1/2 -translate-x-1/2 z-50 bg-red-500 text-white px-6 py-3 border-4 border-black shadow-brutal font-mono text-sm">
                    ⚠️ { errorMessage }
                </div>
            ) }
            <LoginForm onLogin={ handleLogin } onGoToSignup={ handleGoToSignup } />
        </div>
    );
}

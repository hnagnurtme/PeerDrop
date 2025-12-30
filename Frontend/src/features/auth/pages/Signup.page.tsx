import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import SignupForm from '../components/SignupForm';
import { authService } from '../services';
import { useAuthStore } from '../store';

interface SignupData {
    username: string;
    email: string;
    password: string;
}

export default function SignupPage () {
    const navigate = useNavigate();
    const { login, setLoading, setError } = useAuthStore();
    const [ errorMessage, setErrorMessage ] = useState<string | null>( null );

    const handleSignup = async ( data: SignupData ) => {
        try {
            setLoading( true );
            setErrorMessage( null );

            const response = await authService.signup( data );
            login( response.user, response.token );
            navigate( '/' );
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Signup failed';
            setErrorMessage( message );
            setError( message );
        } finally {
            setLoading( false );
        }
    };

    const handleBackToLogin = () => {
        navigate( '/login' );
    };

    return (
        <div>
            { errorMessage && (
                <div className="fixed top-20 left-1/2 -translate-x-1/2 z-50 bg-red-500 text-white px-6 py-3 border-4 border-black shadow-brutal font-mono text-sm">
                    ⚠️ { errorMessage }
                </div>
            ) }
            <SignupForm onSignup={ handleSignup } onBackToLogin={ handleBackToLogin } />
        </div>
    );
}

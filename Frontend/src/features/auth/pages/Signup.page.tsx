import { useNavigate } from 'react-router-dom';
import SignupForm from '../components/SignupForm';
import { authService } from '../services';
import { useAuthStore } from '../store';
import { toast } from '@/shared/components/Toaster';

interface SignupData {
    userName: string;
    fullName: string;
    email: string;
    password: string;
}

export default function SignupPage () {
    const navigate = useNavigate();
    const { login, setLoading, setError } = useAuthStore();

    const handleSignup = async ( data: SignupData ) => {
        try {
            setLoading( true );

            const response = await authService.signup( data );
            login( response.user, response.accessToken );
            navigate( '/' );
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Signup failed';
            toast.error( message );
            setError( message );
        } finally {
            setLoading( false );
        }
    };

    const handleBackToLogin = () => {
        navigate( '/login' );
    };

    return (
        <SignupForm onSignup={ handleSignup } onBackToLogin={ handleBackToLogin } />
    );
}

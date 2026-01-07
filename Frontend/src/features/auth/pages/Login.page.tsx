import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/LoginForm';
import { authService } from '../services';
import { useAuthStore } from '../store';
import { toast } from '@/shared/components/Toaster';

export default function LoginPage () {
    const navigate = useNavigate();
    const { login, setLoading, setError } = useAuthStore();

    const handleLogin = async ( email: string, password: string ) => {
        try {
            setLoading( true );

            const response = await authService.login( { email, password } );
            login( response.user, response.accessToken );
            navigate( '/' );
        } catch ( err ) {
            const message = err instanceof Error ? err.message : 'Login failed';
            toast.error( message );
            setError( message );
        } finally {
            setLoading( false );
        }
    };

    const handleGoToSignup = () => {
        navigate( '/signup' );
    };

    return (
        <LoginForm onLogin={ handleLogin } onGoToSignup={ handleGoToSignup } />
    );
}

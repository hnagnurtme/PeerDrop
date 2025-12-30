
import React, { useState } from 'react';
import { Icons } from '@/shared/constants/Icon';
import Button from '@/shared/components/Button/Button';
import CyberBranding from './CyberBranding';
import InputField from '@/shared/components/InputField';

interface LoginProps {
    onLogin: ( email: string, password: string ) => void;
    onGoToSignup: () => void;
}

const LoginForm: React.FC<LoginProps> = ( { onLogin, onGoToSignup } ) => {
    const [ email, setEmail ] = useState( '' );
    const [ password, setPassword ] = useState( '' );
    const [ isAuthenticating, setIsAuthenticating ] = useState( false );
    const [ showPassword, setShowPassword ] = useState( false );

    const handleSubmit = ( e: React.FormEvent ) => {
        e.preventDefault();
        if ( !email.trim() || !password.trim() ) return;

        setIsAuthenticating( true );
        setTimeout( () => {
            onLogin( email, password );
        }, 800 );
    };

    return (
        <main className="relative z-10 flex-grow flex items-center justify-center p-4 md:p-8">
            <div className="w-full max-w-5xl flex flex-col md:flex-row border-4 border-black bg-white shadow-brutal-lg overflow-hidden">

                <CyberBranding
                    theme="login"
                    bgText="PORT"
                    statusLabel="System.Link.Active"
                    title={ <>Secure<br />Access<br /><span className="text-primary italic">Required</span></> }
                    terminalLines={ [
                        { label: 'ENCRYPTION', value: 'AES-256-GCM' },
                        { label: 'STATUS', value: 'WAITING_HANDSHAKE' },
                        { label: 'WARNING', value: 'AUTHORIZED AGENTS ONLY', isHighlight: true }
                    ] }
                />

                {/* Right Section: Form */ }
                <div className="w-full md:w-7/12 p-8 md:p-16 flex flex-col justify-center bg-beige-bg">
                    <div className="mb-10">
                        <h3 className="text-4xl font-black uppercase tracking-tight mb-2">Login</h3>
                        <p className="text-gray-500 font-mono text-xs font-bold">Enter your credentials to continue //</p>
                    </div>

                    <form onSubmit={ handleSubmit } className="space-y-8">
                        <InputField
                            label="Email"
                            icon={ <Icons.Email /> }
                            type="email"
                            value={ email }
                            onChange={ ( e ) => setEmail( e.target.value ) }
                            placeholder="your@email.com"
                            helperText="Enter your email address"
                            required
                        />

                        <InputField
                            label="Password"
                            icon={
                                <button
                                    type="button"
                                    onClick={ () => setShowPassword( !showPassword ) }
                                    className="hover:text-secondary transition-colors flex items-center"
                                >
                                    { showPassword ? <Icons.Visibility /> : <Icons.VisibilityOff /> }
                                </button>
                            }
                            type={ showPassword ? "text" : "password" }
                            value={ password }
                            onChange={ ( e ) => setPassword( e.target.value ) }
                            placeholder="••••••••"
                            helperText="Enter your password"
                            required
                        />

                        <Button
                            type="submit"
                            size="lg"
                            className={ `${ isAuthenticating ? 'opacity-80 pointer-events-none' : '' }` }
                            icon={ !isAuthenticating ? <Icons.ArrowForward /> : undefined }
                        >
                            { isAuthenticating ? (
                                <span className="flex items-center gap-4 animate-pulse">AUTHENTICATING...</span>
                            ) : (
                                <span className="glitch-hover">Execute Login</span>
                            ) }
                        </Button>
                    </form>

                    <button
                        onClick={ onGoToSignup }
                        className="mt-8 text-sm font-black uppercase py-4 border-4 border-dashed border-black/20 hover:border-secondary hover:text-secondary hover:bg-secondary/5 transition-all active-press"
                    >
                        Initialize New Agent Node
                    </button>
                </div>
            </div>
        </main>
    );
};

export default LoginForm;

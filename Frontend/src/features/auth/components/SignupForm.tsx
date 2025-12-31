
import React, { useState } from 'react';
import { Icons } from '@/shared/constants/Icon';
import Button from '@/shared/components/Button/Button';
import CyberBranding from './CyberBranding';
import InputField from '@/shared/components/InputField';

interface SignupData {
    userName: string;
    fullName: string;
    email: string;
    password: string;
}

interface SignupProps {
    onSignup: ( data: SignupData ) => void;
    onBackToLogin: () => void;
}

const SignupForm: React.FC<SignupProps> = ( { onSignup, onBackToLogin } ) => {
    const [ formData, setFormData ] = useState( {
        userName: '',
        fullName: '',
        email: '',
        password: '',
        confirmPassword: ''
    } );
    const [ isRegistering, setIsRegistering ] = useState( false );
    const [ showPassword, setShowPassword ] = useState( false );

    const handleSubmit = ( e: React.FormEvent ) => {
        e.preventDefault();
        if ( !formData.userName.trim() || !formData.fullName.trim() || !formData.email.trim() || !formData.password.trim() ) return;
        if ( formData.password !== formData.confirmPassword ) return;

        setIsRegistering( true );
        onSignup( {
            userName: formData.userName,
            fullName: formData.fullName,
            email: formData.email,
            password: formData.password
        } );
    };

    return (
        <main className="relative z-10 flex-grow flex items-center justify-center p-4 md:p-8">
            <div className="w-full max-w-5xl flex flex-col md:flex-row border-4 border-black bg-white shadow-brutal-lg overflow-hidden">

                <CyberBranding
                    theme="signup"
                    bgText="NODE"
                    statusLabel="Node_Creation_Wizard"
                    title={ <>Join The<br /><span className="bg-black text-primary px-2">Network</span><br />Uplink</> }
                    terminalLines={ [
                        { label: 'UPLINK_PROTOCOL', value: 'WEB_SOCKET_SECURE' },
                        { label: 'NODE_TYPE', value: 'END_POINT_ASSET' },
                        { label: 'VISIBILITY', value: 'STEALTH_ACTIVE', isHighlight: true }
                    ] }
                />

                {/* Right Section: Form */ }
                <div className="w-full md:w-7/12 p-8 md:p-12 flex flex-col justify-center bg-beige-bg">
                    <div className="mb-8">
                        <h3 className="text-3xl font-black uppercase tracking-tight mb-2">Sign Up</h3>
                        <div className="h-2 w-20 bg-primary border-2 border-black"></div>
                    </div>

                    <form onSubmit={ handleSubmit } className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="md:col-span-2">
                            <InputField
                                label="Username"
                                icon={ <Icons.Badge /> }
                                required
                                placeholder="e.g. johndoe"
                                value={ formData.userName }
                                onChange={ e => setFormData( { ...formData, userName: e.target.value } ) }
                                helperText="Your username (3-20 characters, letters, numbers, underscores)"
                            />
                        </div>

                        <div className="md:col-span-2">
                            <InputField
                                label="Full Name"
                                icon={ <Icons.Badge /> }
                                required
                                placeholder="e.g. John Doe"
                                value={ formData.fullName }
                                onChange={ e => setFormData( { ...formData, fullName: e.target.value } ) }
                                helperText="Your full name"
                            />
                        </div>

                        <div className="md:col-span-2">
                            <InputField
                                label="Email"
                                icon={ <Icons.Email /> }
                                type="email"
                                required
                                placeholder="your@email.com"
                                value={ formData.email }
                                onChange={ e => setFormData( { ...formData, email: e.target.value } ) }
                                helperText="Enter your email address"
                            />
                        </div>

                        <div>
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
                                required
                                placeholder="••••••••"
                                value={ formData.password }
                                onChange={ e => setFormData( { ...formData, password: e.target.value } ) }
                                helperText="Min 6 chars, 1 uppercase, 1 number"
                            />
                        </div>

                        <div>
                            <InputField
                                label="Confirm Password"
                                type={ showPassword ? "text" : "password" }
                                required
                                placeholder="••••••••"
                                value={ formData.confirmPassword }
                                onChange={ e => setFormData( { ...formData, confirmPassword: e.target.value } ) }
                                error={ formData.password && formData.confirmPassword && formData.password !== formData.confirmPassword ? "Passwords do not match" : undefined }
                                helperText="Re-enter your password"
                            />
                        </div>

                        <div className="md:col-span-2 pt-4">
                            <Button
                                type="submit"
                                className={ isRegistering ? 'opacity-80 pointer-events-none' : '' }
                            >
                                { isRegistering ? (
                                    <span className="animate-pulse">Creating account...</span>
                                ) : (
                                    <span className="flex items-center gap-3">
                                        Sign Up
                                        <Icons.ArrowForward />
                                    </span>
                                ) }
                            </Button>
                        </div>
                    </form>

                    <button
                        onClick={ onBackToLogin }
                        className="mt-8 text-center font-black uppercase text-xs hover:text-secondary transition-colors underline decoration-2 underline-offset-4"
                    >
                        Already have an account? Login
                    </button>
                </div>
            </div>
        </main>
    );
};

export default SignupForm;

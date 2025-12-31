import type { LoginFormData, SignupFormData } from '../schemas';
import type { User } from '../store';
import type { AuthResponse } from './auth.service';

// Simulated delay to mimic network latency
const delay = ( ms: number ) => new Promise( resolve => setTimeout( resolve, ms ) );

// Mock user database
const mockUsers: Map<string, { user: User; password: string }> = new Map( [
    [ 'demo@pixelport.sys', {
        user: {
            id: '1',
            email: 'demo@pixelport.sys',
            userName: 'AGENT_DEMO',
            fullName: 'Demo Agent',
            role: 'User',
            avatar: undefined,
        },
        password: 'Demo123'
    } ],
    [ 'admin@pixelport.sys', {
        user: {
            id: '2',
            email: 'admin@pixelport.sys',
            userName: 'ADMIN_NODE',
            fullName: 'Admin Node',
            role: 'Admin',
            avatar: undefined,
        },
        password: 'Admin123'
    } ]
] );

// Generate mock JWT token
const generateToken = ( userId: string ): string => {
    const header = btoa( JSON.stringify( { alg: 'HS256', typ: 'JWT' } ) );
    const payload = btoa( JSON.stringify( {
        sub: userId,
        iat: Date.now(),
        exp: Date.now() + 24 * 60 * 60 * 1000
    } ) );
    const signature = btoa( 'mock_signature' );
    return `${ header }.${ payload }.${ signature }`;
};

// Generate unique ID
const generateId = (): string => {
    return Math.random().toString( 36 ).substring( 2, 15 );
};

export const mockAuthService = {
    async login ( data: LoginFormData ): Promise<AuthResponse> {
        await delay( 800 );

        const stored = mockUsers.get( data.email.toLowerCase() );

        if ( !stored ) {
            throw new Error( 'User not found. Try demo@pixelport.sys / Demo123' );
        }

        if ( stored.password !== data.password ) {
            throw new Error( 'Invalid password. Try Demo123' );
        }

        return {
            accessToken: generateToken( stored.user.id ),
            refreshToken: generateToken( stored.user.id ),
            expiresAt: new Date( Date.now() + 24 * 60 * 60 * 1000 ).toISOString(),
            user: stored.user
        };
    },

    async signup ( data: Omit<SignupFormData, 'confirmPassword'> ): Promise<AuthResponse> {
        await delay( 1000 );

        if ( mockUsers.has( data.email.toLowerCase() ) ) {
            throw new Error( 'Email already registered' );
        }

        const newUser: User = {
            id: generateId(),
            email: data.email.toLowerCase(),
            userName: data.userName,
            fullName: data.fullName,
            role: 'User',
            avatar: undefined,
        };

        mockUsers.set( data.email.toLowerCase(), {
            user: newUser,
            password: data.password
        } );

        return {
            accessToken: generateToken( newUser.id ),
            refreshToken: generateToken( newUser.id ),
            expiresAt: new Date( Date.now() + 24 * 60 * 60 * 1000 ).toISOString(),
            user: newUser
        };
    },

    async logout (): Promise<void> {
        await delay( 300 );
    },

    async getCurrentUser (): Promise<User> {
        await delay( 500 );

        const stored = mockUsers.get( 'demo@pixelport.sys' );
        if ( stored ) {
            return stored.user;
        }
        throw new Error( 'Not authenticated' );
    },

    async refreshToken (): Promise<{ token: string }> {
        await delay( 300 );
        return { token: generateToken( '1' ) };
    },

    async forgotPassword ( email: string ): Promise<{ message: string }> {
        await delay( 800 );

        if ( !mockUsers.has( email.toLowerCase() ) ) {
            throw new Error( 'Email not found' );
        }

        return { message: 'Password reset link sent to your email' };
    },

    async resetPassword ( _token: string, _password: string ): Promise<{ message: string }> {
        await delay( 800 );
        return { message: 'Password reset successfully' };
    },
};

export default mockAuthService;

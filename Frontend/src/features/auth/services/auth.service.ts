import type { LoginFormData, SignupFormData } from '../schemas';
import type { User } from '../store';

export interface AuthResponse {
    user: User;
    token: string;
    message?: string;
}

export interface ApiError {
    message: string;
    statusCode: number;
}


const USE_MOCK = true;

// Lazy load the appropriate service
const getService = async () => {
    if ( USE_MOCK ) {
        const { mockAuthService } = await import( './mock.service' );
        return mockAuthService;
    } else {
        const axios = await import( 'axios' );
        const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:3000/api';

        const authApi = axios.default.create( {
            baseURL: `${ API_BASE_URL }/auth`,
            headers: {
                'Content-Type': 'application/json',
            },
        } );

        authApi.interceptors.request.use( ( config ) => {
            const token = localStorage.getItem( 'auth-storage' );
            if ( token ) {
                try {
                    const parsed = JSON.parse( token );
                    if ( parsed.state?.token ) {
                        config.headers.Authorization = `Bearer ${ parsed.state.token }`;
                    }
                } catch {
                    // Invalid token format
                }
            }
            return config;
        } );

        return {
            async login ( data: LoginFormData ): Promise<AuthResponse> {
                const response = await authApi.post<AuthResponse>( '/login', data );
                return response.data;
            },
            async signup ( data: Omit<SignupFormData, 'confirmPassword'> ): Promise<AuthResponse> {
                const response = await authApi.post<AuthResponse>( '/signup', data );
                return response.data;
            },
            async logout (): Promise<void> {
                await authApi.post( '/logout' );
            },
            async getCurrentUser (): Promise<User> {
                const response = await authApi.get<User>( '/me' );
                return response.data;
            },
            async refreshToken (): Promise<{ token: string }> {
                const response = await authApi.post<{ token: string }>( '/refresh' );
                return response.data;
            },
            async forgotPassword ( email: string ): Promise<{ message: string }> {
                const response = await authApi.post<{ message: string }>( '/forgot-password', { email } );
                return response.data;
            },
            async resetPassword ( token: string, password: string ): Promise<{ message: string }> {
                const response = await authApi.post<{ message: string }>( '/reset-password', { token, password } );
                return response.data;
            },
        };
    }
};

// Create a proxy that lazily loads the service
export const authService = {
    async login ( data: LoginFormData ): Promise<AuthResponse> {
        const service = await getService();
        return service.login( data );
    },

    async signup ( data: Omit<SignupFormData, 'confirmPassword'> ): Promise<AuthResponse> {
        const service = await getService();
        return service.signup( data );
    },

    async logout (): Promise<void> {
        const service = await getService();
        return service.logout();
    },

    async getCurrentUser (): Promise<User> {
        const service = await getService();
        return service.getCurrentUser();
    },

    async refreshToken (): Promise<{ token: string }> {
        const service = await getService();
        return service.refreshToken();
    },

    async forgotPassword ( email: string ): Promise<{ message: string }> {
        const service = await getService();
        return service.forgotPassword( email );
    },

    async resetPassword ( token: string, password: string ): Promise<{ message: string }> {
        const service = await getService();
        return service.resetPassword( token, password );
    },
};

export default authService;

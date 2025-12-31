import type { LoginFormData, SignupFormData } from '../schemas';
import type { User } from '../store';

export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
    user: User;
}

export interface ApiError {
    message: string;
    statusCode: number;
}



const getService = async () => {
    const apiClient = ( await import( '@/shared/lib/apiClient' ) ).default;

    // Create auth-specific client with /auth base path
    const authApi = {
        async get<T> ( url: string ) {
            return apiClient.get<T>( `/auth${ url }` );
        },
        async post<T> ( url: string, data?: any ) {
            return apiClient.post<T>( `/auth${ url }`, data );
        },
    };

    return {
        async login ( data: LoginFormData ): Promise<AuthResponse> {
            const response = await authApi.post<{ isSuccess: boolean; data: AuthResponse }>( '/login', data );
            return response.data.data;
        },
        async signup ( data: Omit<SignupFormData, 'confirmPassword'> ): Promise<AuthResponse> {
            const response = await authApi.post<{ isSuccess: boolean; data: AuthResponse }>( '/register', {
                email: data.email,
                userName: data.userName,
                password: data.password,
                confirmPassword: data.password,
                fullName: data.fullName
            } );
            return response.data.data;
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
};

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

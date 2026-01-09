import { environment } from '@/environments/environment';

export const API_CONFIG = {
    baseUrl: environment.apiUrl,

    endpoints: {
        auth: {
            login: 'auth/login',
            logout: 'auth/logout',
            refresh: 'auth/refresh',
        },
        users: {
            profile: 'users/me',
        },
    },
};

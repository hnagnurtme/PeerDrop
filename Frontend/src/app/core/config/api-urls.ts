import { buildApiUrl } from '@/app/core/services/url-builder.service';
import { API_CONFIG } from './api-config';

export const API_URLS = {
    auth: {
        login: () => buildApiUrl( API_CONFIG.endpoints.auth.login ),
        logout: () => buildApiUrl( API_CONFIG.endpoints.auth.logout ),
        refresh: () => buildApiUrl( API_CONFIG.endpoints.auth.refresh ),
    },
    users: {
        profile: () => buildApiUrl( API_CONFIG.endpoints.users.profile ),
    },
};

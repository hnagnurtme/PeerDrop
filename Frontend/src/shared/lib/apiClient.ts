import axios from 'axios';
import type { AxiosInstance, AxiosError } from 'axios';

// Get API base URL from environment
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8080/api';

// Create axios instance with default config
const apiClient: AxiosInstance = axios.create( {
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 10000, // 10 seconds
} );

// Request interceptor - Add auth token to requests
apiClient.interceptors.request.use(
    ( config ) => {
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
    },
    ( error ) => {
        return Promise.reject( error );
    }
);

// Response interceptor - Handle errors and extract messages
apiClient.interceptors.response.use(
    ( response ) => response,
    ( error: AxiosError ) => {
        // Extract message from backend ApiResponse
        if ( error.response?.data ) {
            const data = error.response.data as any;
            // Backend returns { isSuccess: false, message: "...", errors: {...} }
            if ( data.message ) {
                // Create a new error with the backend message
                const backendError = new Error( data.message );
                return Promise.reject( backendError );
            }
        }

        // Fallback to original error
        return Promise.reject( error );
    }
);

export default apiClient;

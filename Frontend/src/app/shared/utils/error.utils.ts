import { HttpErrorResponse } from '@angular/common/http';

/**
 * Converts HttpErrorResponse to user-friendly error messages.
 * Handles network errors, CORS issues, and common HTTP status codes.
 */
export function getHttpErrorMessage ( err: HttpErrorResponse ): string {
    // Network error or CORS issue
    if ( err.status === 0 ) {
        return 'Unable to connect to server. Please check your internet connection.';
    }

    // Server returned an error message
    if ( err.error?.message ) {
        return err.error.message;
    }

    // Handle common HTTP status codes
    switch ( err.status ) {
        case 400:
            return 'Invalid request. Please check your input.';
        case 401:
            return 'Authentication failed. Please log in again.';
        case 403:
            return 'Access denied. You don\'t have permission.';
        case 404:
            return 'Resource not found.';
        case 409:
            return 'Conflict occurred. Please refresh and try again.';
        case 422:
            return 'Validation failed. Please check your input.';
        case 429:
            return 'Too many requests. Please wait and try again.';
        case 500:
        case 502:
        case 503:
        case 504:
            return 'Server error. Please try again later.';
        default:
            return 'An unexpected error occurred. Please try again.';
    }
}

/**
 * Get context-specific error messages for authentication.
 */
export function getAuthErrorMessage ( err: HttpErrorResponse ): string {
    if ( err.status === 0 ) {
        return 'Unable to connect to server. Please check your internet connection.';
    }

    if ( err.error?.message ) {
        return err.error.message;
    }

    switch ( err.status ) {
        case 401:
            return 'Invalid email or password.';
        case 403:
            return 'Account is locked or disabled. Please contact support.';
        case 404:
            return 'Account not found.';
        case 429:
            return 'Too many login attempts. Please try again later.';
        default:
            return getHttpErrorMessage( err );
    }
}

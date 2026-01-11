import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

// ============================================================================
// Validation Patterns
// ============================================================================
export const AUTH_VALIDATION_PATTERNS = {
    EMAIL: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
    PASSWORD: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,}$/,
    USERNAME: /^[a-zA-Z0-9_]+$/
} as const;

export const AUTH_VALIDATION_LIMITS = {
    EMAIL_MAX_LENGTH: 255,
    PASSWORD_MIN_LENGTH: 8,
    PASSWORD_MAX_LENGTH: 128,
    USERNAME_MIN_LENGTH: 3,
    USERNAME_MAX_LENGTH: 30,
    FULLNAME_MIN_LENGTH: 2,
    FULLNAME_MAX_LENGTH: 50
} as const;

// ============================================================================
// Simple Validator Arrays (for quick use)
// ============================================================================
export const emailValidator = [
    Validators.required,
    Validators.email
];

export const passwordValidator = [
    Validators.required,
    Validators.minLength( AUTH_VALIDATION_LIMITS.PASSWORD_MIN_LENGTH )
];

export const fullNameValidator = [
    Validators.required,
    Validators.minLength( AUTH_VALIDATION_LIMITS.FULLNAME_MIN_LENGTH ),
    Validators.maxLength( AUTH_VALIDATION_LIMITS.FULLNAME_MAX_LENGTH )
];

export const userNameValidator = [
    Validators.required,
    Validators.minLength( AUTH_VALIDATION_LIMITS.USERNAME_MIN_LENGTH ),
    Validators.maxLength( AUTH_VALIDATION_LIMITS.USERNAME_MAX_LENGTH ),
    Validators.pattern( AUTH_VALIDATION_PATTERNS.USERNAME )
];

// ============================================================================
// Custom Validator Functions (for complex validation logic)
// ============================================================================
export class AuthValidators {
    /**
     * Validates email format with max length check
     */
    static email ( control: AbstractControl ): ValidationErrors | null {
        if ( !control.value ) return null;

        const value = control.value as string;

        if ( value.length > AUTH_VALIDATION_LIMITS.EMAIL_MAX_LENGTH ) {
            return { maxlength: true };
        }

        if ( !AUTH_VALIDATION_PATTERNS.EMAIL.test( value ) ) {
            return { email: true };
        }

        return null;
    }

    /**
     * Validates password with pattern (letters + numbers required)
     */
    static password ( control: AbstractControl ): ValidationErrors | null {
        if ( !control.value ) return null;

        const value = control.value as string;

        if ( value.length < AUTH_VALIDATION_LIMITS.PASSWORD_MIN_LENGTH ) {
            return { minlength: true };
        }

        if ( !AUTH_VALIDATION_PATTERNS.PASSWORD.test( value ) ) {
            return { pattern: true };
        }

        return null;
    }
}

// ============================================================================
// Cross-field Validators
// ============================================================================
export const confirmPasswordValidator: ValidatorFn = ( control: AbstractControl ): ValidationErrors | null => {
    const password = control.get( 'password' );
    const confirmPassword = control.get( 'confirmPassword' );

    if ( !password || !confirmPassword ) return null;

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
};

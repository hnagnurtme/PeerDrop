import { AbstractControl, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

export const emailValidator = [
    Validators.required,
    Validators.email
];

export const passwordValidator = [
    Validators.required,
    Validators.minLength( 6 )
];

export const fullNameValidator = [
    Validators.required,
    Validators.minLength( 2 ),
    Validators.maxLength( 50 )
];

export const userNameValidator = [
    Validators.required,
    Validators.minLength( 3 ),
    Validators.maxLength( 30 ),
    Validators.pattern( /^[a-zA-Z0-9_]+$/ )
];

export const confirmPasswordValidator: ValidatorFn = ( control: AbstractControl ): ValidationErrors | null => {
    const password = control.get( 'password' );
    const confirmPassword = control.get( 'confirmPassword' );

    if ( !password || !confirmPassword ) return null;

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
};

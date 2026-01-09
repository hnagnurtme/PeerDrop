import { Validators } from '@angular/forms';

export const emailValidator = [
    Validators.required,
    Validators.email
];

export const passwordValidator = [
    Validators.required,
    Validators.minLength( 6 )
];

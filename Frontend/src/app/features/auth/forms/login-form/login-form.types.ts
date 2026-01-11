import { FormControl } from "@angular/forms";

// Type-safe form controls
export type LoginFormControls = {
    email: FormControl<string>;
    password: FormControl<string>;
    rememberMe: FormControl<boolean>;
};

// Form value interface
export interface LoginFormValue {
    email: string;
    password: string;
    rememberMe: boolean;
}

// Emit data to parent
export interface LoginFormSubmit {
    email: string;
    password: string;
}

// Component input properties
export interface LoginFormConfig {
    isLoading?: boolean;
    autofocus?: boolean;
    translations?: LoginFormTranslations;
    enableRememberMe?: boolean;
    enablePasswordToggle?: boolean;
}

// i18n support
export interface LoginFormTranslations {
    emailLabel?: string;
    emailPlaceholder?: string;
    passwordLabel?: string;
    passwordPlaceholder?: string;
    rememberMe?: string;
    forgotPassword?: string;
    submitButton?: string;
    authenticating?: string;
    showPassword?: string;
    hidePassword?: string;
    emailRequired?: string;
    emailInvalid?: string;
    passwordRequired?: string;
    passwordMinLength?: string;
    passwordPattern?: string;
}
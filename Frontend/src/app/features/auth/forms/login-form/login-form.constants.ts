import { LoginFormTranslations } from './login-form.types';

// Default form values
export const LOGIN_FORM_DEFAULTS = {
    EMAIL: '',
    PASSWORD: '',
    REMEMBER_ME: false
} as const;

// Default translations (English)
export const DEFAULT_TRANSLATIONS: Required<LoginFormTranslations> = {
    // Labels
    emailLabel: 'Email Address',
    emailPlaceholder: 'Enter your email',
    passwordLabel: 'Password',
    passwordPlaceholder: 'Enter your password',

    // Checkbox
    rememberMe: 'Remember this device',

    // Links
    forgotPassword: 'Forgot password?',

    // Buttons
    submitButton: 'Sign In',
    authenticating: 'Signing in...',

    // Password toggle
    showPassword: 'Show password',
    hidePassword: 'Hide password',

    // Error messages
    emailRequired: 'Email is required',
    emailInvalid: 'Please enter a valid email address',
    passwordRequired: 'Password is required',
    passwordMinLength: 'Password must be at least 8 characters',
    passwordPattern: 'Password must contain letters and numbers',
} as const;

// Vietnamese translations (example for i18n)
export const VIETNAMESE_TRANSLATIONS: Required<LoginFormTranslations> = {
    emailLabel: 'Địa chỉ email',
    emailPlaceholder: 'Nhập email của bạn',
    passwordLabel: 'Mật khẩu',
    passwordPlaceholder: 'Nhập mật khẩu của bạn',
    rememberMe: 'Ghi nhớ thiết bị này',
    forgotPassword: 'Quên mật khẩu?',
    submitButton: 'Đăng nhập',
    authenticating: 'Đang đăng nhập...',
    showPassword: 'Hiển thị mật khẩu',
    hidePassword: 'Ẩn mật khẩu',
    emailRequired: 'Email là bắt buộc',
    emailInvalid: 'Vui lòng nhập địa chỉ email hợp lệ',
    passwordRequired: 'Mật khẩu là bắt buộc',
    passwordMinLength: 'Mật khẩu phải có ít nhất 8 ký tự',
    passwordPattern: 'Mật khẩu phải chứa chữ cái và số',
} as const;

// Error messages categorized
export const ERROR_MESSAGES = {
    EMAIL: {
        REQUIRED: DEFAULT_TRANSLATIONS.emailLabel + ' is required',
        INVALID: DEFAULT_TRANSLATIONS.emailLabel + ' is invalid',
        MAX_LENGTH: 'Email cannot exceed 255 characters',
        NOT_FOUND: 'Email address not found',
        DISABLED: 'This account has been disabled'
    },
    PASSWORD: {
        REQUIRED: DEFAULT_TRANSLATIONS.passwordLabel + ' is required',
        MIN_LENGTH: DEFAULT_TRANSLATIONS.passwordLabel + ' must be at least 8 characters',
        PATTERN: DEFAULT_TRANSLATIONS.passwordLabel + ' must contain letters and numbers',
        INCORRECT: 'Incorrect password',
        EXPIRED: 'Password has expired',
        LOCKED: 'Account is locked due to too many failed attempts'
    },
    GENERAL: {
        NETWORK_ERROR: 'Network error. Please check your connection',
        SERVER_ERROR: 'Server error. Please try again later',
        TIMEOUT: 'Request timeout. Please try again',
        MAINTENANCE: 'System under maintenance. Please try again later'
    }
} as const;



// Form configuration defaults
export const FORM_CONFIG_DEFAULTS = {
    MAX_EMAIL_LENGTH: 255,
    MAX_PASSWORD_LENGTH: 128,
    AUTO_FOCUS_DELAY: 100,
    REMEMBER_ME_EXPIRY: 30 * 24 * 60 * 60 * 1000 // 30 days in milliseconds
} as const;
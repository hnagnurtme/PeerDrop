import {
    Component,
    EventEmitter,
    Output,
    Input,
    OnInit,
    OnChanges,
    OnDestroy,
    SimpleChanges,
    inject,
    ChangeDetectionStrategy,
    ViewChild,
    HostListener,
    DestroyRef
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from '@angular/forms';
import {
    LoginFormControls,
    LoginFormValue,
    LoginFormSubmit,
    LoginFormConfig,
    LoginFormTranslations
} from './login-form.types';
import {
    ERROR_MESSAGES,
    DEFAULT_TRANSLATIONS,
    FORM_CONFIG_DEFAULTS
} from './login-form.constants';
import { AuthValidators, AUTH_VALIDATION_LIMITS } from '@/app/core/auth';
import { AuthInputComponent, AuthSubmitButtonComponent } from '@/app/features/auth/components';
import { StorageService } from '@/app/core/services/storage.service';

@Component( {
    selector: 'app-login-form',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        AuthInputComponent,
        AuthSubmitButtonComponent
    ],
    templateUrl: './login-form.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
} )
export class LoginFormComponent implements OnInit, OnChanges, OnDestroy {
    @Input() config: LoginFormConfig = {};
    @Output() formSubmit = new EventEmitter<LoginFormSubmit>();
    @Output() forgotPasswordClick = new EventEmitter<void>();
    @Output() formValueChange = new EventEmitter<Partial<LoginFormValue>>();
    @ViewChild( 'emailInputRef', { static: false } )
    emailInputRef?: AuthInputComponent;
    private fb = inject( FormBuilder );
    private destroyRef = inject( DestroyRef );
    private storageService = inject( StorageService );
    loginForm: FormGroup<LoginFormControls>;
    _translations: Required<LoginFormTranslations>;
    showPassword = false;
    private autoFocusTimeout?: any;

    constructor () {
        this._translations = this.mergeTranslations();
        this.loginForm = this.createForm();
    }

    ngOnInit (): void {
        this.setupAutoFocus();
        this.setupFormSubscriptions();
        this.loadRememberedCredentials();
    }

    ngOnChanges ( changes: SimpleChanges ): void {
        if ( changes[ 'config' ] ) {
            this._translations = this.mergeTranslations();
            this.updateFormDisabledState();
        }
    }

    ngOnDestroy (): void {
        // Cleanup timeout to prevent memory leaks
        if ( this.autoFocusTimeout ) {
            clearTimeout( this.autoFocusTimeout );
        }
    }
    get emailControl (): FormControl<string> {
        return this.loginForm.controls.email;
    }

    get passwordControl (): FormControl<string> {
        return this.loginForm.controls.password;
    }

    get rememberMeControl (): FormControl<boolean> {
        return this.loginForm.controls.rememberMe;
    }

    get emailError (): string {
        const control = this.emailControl;

        if ( !control.touched && !control.dirty ) {
            return '';
        }

        if ( control.hasError( 'required' ) ) {
            return this._translations.emailRequired;
        }

        if ( control.hasError( 'email' ) ) {
            return this._translations.emailInvalid;
        }

        if ( control.hasError( 'maxlength' ) ) {
            return ERROR_MESSAGES.EMAIL.MAX_LENGTH;
        }

        return '';
    }

    get passwordError (): string {
        const control = this.passwordControl;

        if ( !control.touched && !control.dirty ) {
            return '';
        }

        if ( control.hasError( 'required' ) ) {
            return this._translations.passwordRequired;
        }

        if ( control.hasError( 'minlength' ) ) {
            return this._translations.passwordMinLength;
        }

        if ( control.hasError( 'pattern' ) ) {
            return this._translations.passwordPattern;
        }

        return '';
    }

    get isLoading (): boolean {
        return this.config.isLoading || false;
    }

    get shouldEnableRememberMe (): boolean {
        return this.config.enableRememberMe !== false;
    }

    get shouldEnablePasswordToggle (): boolean {
        return this.config.enablePasswordToggle !== false;
    }

    get passwordInputType (): 'text' | 'password' {
        return this.showPassword ? 'text' : 'password';
    }

    get passwordToggleIcon (): string {
        return this.showPassword ? 'visibility_off' : 'visibility';
    }

    get passwordToggleLabel (): string {
        return this.showPassword
            ? this._translations.hidePassword
            : this._translations.showPassword;
    }
    onSubmit (): void {
        if ( this.loginForm.valid && !this.isLoading ) {
            this.saveRememberedCredentials();

            const formValue = this.loginForm.getRawValue();
            this.formSubmit.emit( {
                email: formValue.email,
                password: formValue.password
            } );
        } else {
            this.markFormAsTouched();
        }
    }

    resetForm (): void {
        this.loginForm.reset( {
            email: '',
            password: '',
            rememberMe: false
        } );
    }

    togglePasswordVisibility (): void {
        if ( this.shouldEnablePasswordToggle ) {
            this.showPassword = !this.showPassword;
        }
    }

    onForgotPassword (): void {
        if ( !this.isLoading ) {
            this.forgotPasswordClick.emit();
        }
    }

    markFormAsTouched (): void {
        Object.values( this.loginForm.controls ).forEach( control => {
            control.markAsTouched();
        } );
    }

    private createForm (): FormGroup<LoginFormControls> {
        return this.fb.nonNullable.group<LoginFormControls>( {
            email: this.fb.nonNullable.control<string>(
                '',
                [
                    Validators.required,
                    AuthValidators.email
                ]
            ),
            password: this.fb.nonNullable.control<string>(
                '',
                [
                    Validators.required,
                    AuthValidators.password
                ]
            ),
            rememberMe: this.fb.nonNullable.control<boolean>( false )
        } );
    }

    private mergeTranslations (): Required<LoginFormTranslations> {
        return {
            ...DEFAULT_TRANSLATIONS,
            ...this.config.translations
        } as Required<LoginFormTranslations>;
    }

    private setupAutoFocus (): void {
        const shouldAutoFocus = this.config.autofocus !== false;

        if ( shouldAutoFocus && this.emailInputRef ) {
            this.autoFocusTimeout = setTimeout( () => {
                this.emailInputRef?.focus();
            }, FORM_CONFIG_DEFAULTS.AUTO_FOCUS_DELAY );
        }
    }

    private setupFormSubscriptions (): void {
        this.loginForm.valueChanges
            .pipe( takeUntilDestroyed( this.destroyRef ) )
            .subscribe( value => {
                this.formValueChange.emit( value );
            } );
    }

    private updateFormDisabledState (): void {
        if ( this.isLoading ) {
            this.loginForm.disable( { emitEvent: false } );
        } else {
            this.loginForm.enable( { emitEvent: false } );
        }
    }

    private loadRememberedCredentials (): void {
        if ( !this.shouldEnableRememberMe ) {
            return;
        }

        const saved = this.storageService.getRememberedLogin();
        if ( saved ) {
            this.loginForm.patchValue( {
                email: saved.email,
                rememberMe: true
            } );
        }
    }

    private saveRememberedCredentials (): void {
        if ( !this.shouldEnableRememberMe ) {
            return;
        }

        if ( this.rememberMeControl.value ) {
            this.storageService.setRememberedLogin( this.emailControl.value );
        } else {
            this.storageService.removeRememberedLogin();
        }
    }

    @HostListener( 'keydown.enter', [ '$event' ] )
    onEnterKey ( event: Event ): void {
        if ( this.loginForm.valid && !this.isLoading ) {
            event.preventDefault();
            this.onSubmit();
        }
    }

    @HostListener( 'keydown.escape' )
    onEscapeKey (): void {
        this.resetForm();
    }
}
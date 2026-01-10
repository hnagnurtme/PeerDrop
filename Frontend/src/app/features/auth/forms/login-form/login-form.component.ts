import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthInputComponent } from '@/app/features/auth/components/auth-input/auth-input.component';
import { AuthSubmitButtonComponent } from '@/app/features/auth/components/auth-submit-button/auth-submit-button.component';
import { emailValidator, passwordValidator, LoginRequest } from '@/app/core/auth';

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
    styleUrls: [ './login-form.component.scss' ]
} )
export class LoginFormComponent {
    @Output() formSubmit = new EventEmitter<LoginRequest>();

    private fb = inject( FormBuilder );

    loginForm: FormGroup = this.fb.group( {
        email: [ '', emailValidator ],
        password: [ '', passwordValidator ],
        rememberMe: [ false ]
    } );

    get emailError (): string {
        const control = this.loginForm.get( 'email' );
        if ( control?.hasError( 'required' ) ) return 'Email là bắt buộc';
        if ( control?.hasError( 'email' ) ) return 'Email không hợp lệ';
        return '';
    }

    get passwordError (): string {
        const control = this.loginForm.get( 'password' );
        if ( control?.hasError( 'required' ) ) return 'Mật khẩu là bắt buộc';
        if ( control?.hasError( 'minlength' ) ) return 'Mật khẩu phải có ít nhất 6 ký tự';
        return '';
    }

    onSubmit (): void {
        if ( this.loginForm.valid ) {
            const { email, password } = this.loginForm.value;
            this.formSubmit.emit( { email, password } );
        } else {
            this.loginForm.markAllAsTouched();
        }
    }
}

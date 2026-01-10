import { Component, Input, forwardRef, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component( {
    selector: 'auth-input',
    standalone: true,
    imports: [ CommonModule, ReactiveFormsModule ],
    templateUrl: './auth-input.component.html',
    styleUrls: [ './auth-input.component.scss' ],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef( () => AuthInputComponent ),
            multi: true
        }
    ]
} )
export class AuthInputComponent implements ControlValueAccessor {
    @Input() label!: string;
    @Input() placeholder = '';
    @Input() type: 'text' | 'email' | 'password' = 'text';
    @Input() icon!: string;
    @Input() errorMessage = '';

    value = signal<string>( '' );
    disabled = signal<boolean>( false );
    touched = signal<boolean>( false );

    private onChange: ( value: string ) => void = () => { };
    private onTouched: () => void = () => { };

    writeValue ( value: string ): void {
        this.value.set( value || '' );
    }

    registerOnChange ( fn: ( value: string ) => void ): void {
        this.onChange = fn;
    }

    registerOnTouched ( fn: () => void ): void {
        this.onTouched = fn;
    }

    setDisabledState ( isDisabled: boolean ): void {
        this.disabled.set( isDisabled );
    }

    onInput ( event: Event ): void {
        const input = event.target as HTMLInputElement;
        this.value.set( input.value );
        this.onChange( input.value );
    }

    onBlur (): void {
        this.touched.set( true );
        this.onTouched();
    }
}

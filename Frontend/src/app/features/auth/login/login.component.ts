import { Component, inject, Injectable } from "@angular/core";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { AuthFacade, emailValidator, passwordValidator } from "@/app/core/auth";
import { Router } from "@angular/router";
import { CommonModule } from "@angular/common";


@Component({    
    selector: 'app-login',
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent{
    private fb = inject(FormBuilder);
    private authFacade = inject(AuthFacade);
    private router = inject(Router);

    form = this.fb.group({
        email: ['',emailValidator],
        password: ['', passwordValidator]
    });

    submit(){
        if(this.form.invalid) {
            console.log('Form invalid:', this.form.errors);
            console.log('Email errors:', this.form.get('email')?.errors);
            console.log('Password errors:', this.form.get('password')?.errors);
            return;
        }
        console.log('Form Value:', this.form.value);

        const { email, password } = this.form.value;
        
        this.authFacade.login({ 
            email: email!, 
            password: password! 
        }).subscribe({
            next: () => {
                this.router.navigate(['/']);
            },
            error: (err) => {
                console.error('Login failed', err);
            }
        })
    }




    
}
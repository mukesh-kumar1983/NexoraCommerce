import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';

import { AuthService } from '../../../auth/services/auth.service.service';

import { Router } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule

  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {

  

  loginForm = this.fb.group({
    email: [
      'mk_soni@hotmail.com',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      'Admin',
      [
        Validators.required,
        Validators.minLength(3)
      ]
   ]
  });

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  onSubmit(): void {
    
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const payload = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

    this.authService.login(payload)
      .subscribe({
        next: (response: any) => {

          
          if (response.success) {
            this.authService.setUser(response.data);
            this.router.navigate(['./app']);

          }
        },

        error: (error: any) => {
          
          console.error('Login Failed', error);
        }
      });
  }
}


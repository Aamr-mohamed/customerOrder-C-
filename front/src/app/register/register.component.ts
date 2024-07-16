import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  email = '';
  username = '';
  password = '';
  loginError = '';
  constructor(
    private authservice: AuthService,
    private router: Router,
  ) {}

  onRegister() {
    this.authservice
      .register(this.email, this.username, this.password)
      .subscribe(
        (res) => {
          this.router.navigate(['/login']);
        },

        (error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.loginError = 'Invalid email or password. Please try again.';
          } else if (error.status === 404) {
            this.loginError = 'User not found. Please try again.';
          } else if (this.email === '') {
            this.loginError = 'Please enter your email';
          } else if (this.username === '') {
            this.loginError = 'Please enter your Username.';
          } else if (this.password === '' || this.password.length < 6) {
            this.loginError = 'Please enter your password(min 6 characters).';
          } else {
            this.loginError =
              'An unexpected error occurred. Please try again later.';
          }
        },
      );
  }
}

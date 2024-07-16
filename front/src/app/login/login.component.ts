import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';
import { HttpClientModule, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  email = '';
  password = '';
  loginError = '';
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  onLogin() {
    this.authService.login(this.email, this.password).subscribe(
      (res) => {
        if (res.role === 'Sales') {
          this.router.navigate(['/sales']);
        } else if (res.role === 'Customer') {
          this.router.navigate(['/customer']);
        }
      },
      (error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.loginError = 'Invalid email or password. Please try again.';
        } else if (error.status === 404) {
          this.loginError = 'User not found. Please try again.';
        } else if (this.email === '') {
          this.loginError = 'Please enter your email';
        } else if (this.password === '') {
          this.loginError = 'Please enter your password.';
        } else {
          this.loginError =
            'An unexpected error occurred. Please try again later.';
        }
      },
    );
  }
}

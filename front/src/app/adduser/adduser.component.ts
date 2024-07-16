import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService';

@Component({
  selector: 'app-adduser',
  standalone: true,
  imports: [RouterModule, HttpClientModule, FormsModule, CommonModule],
  templateUrl: './adduser.component.html',
  styleUrl: './adduser.component.css'
})
export class AdduserComponent {
  constructor(private authService: AuthService, private router: Router) {}
  email = '';
  username = '';
  password = '';

  ngOnInit() {
  }

  addUser() {
    this.authService
      .register(this.email, this.username, this.password)
      .subscribe((res) => {
        alert('User Added');
      });
  }



  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}

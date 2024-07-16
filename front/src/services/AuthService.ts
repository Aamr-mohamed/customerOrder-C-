import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginResponse, RegisterResponse } from '../types/User';
import { environment } from '../../environment';
import moment from 'moment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}
  private apiUrl = environment.apiUrl;

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/users/login`, {
        email,
        password,
      })
      .pipe(
        tap((res: LoginResponse) => {
          console.log(res);
          this.setSession(res);
        }),
      );
  }

  private setSession(authResult: LoginResponse) {
    const expireAt = new Date(authResult.expiration);
    localStorage.setItem('currentUserId', authResult.id);
    localStorage.setItem('token', authResult.token);
    localStorage.setItem('expireAt', expireAt.toString());
    localStorage.setItem('userRole', authResult.role);
  }

  logout() {
    localStorage.removeItem('currentUserId');
    localStorage.removeItem('token');
    localStorage.removeItem('expireAt');
    localStorage.removeItem('userRole');
  }

  public isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  getExpiration() {
    const expiration = localStorage.getItem('expiresAt');
    if (expiration) {
      return moment(new Date(expiration));
    }
    return moment().subtract(1, 'days');
  }

  register(
    email: string,
    username: string,
    password: string,
  ): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/users/register`, {
      email,
      username,
      password,
    });
  }

  // logout(): Observable<any> {
  //   return this.http.post(`${this.apiUrl}/logout`, {});
  // }
}

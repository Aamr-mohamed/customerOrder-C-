import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { AuthService } from './AuthService';

@Injectable({
  providedIn: 'root',
})
export class AuthRedirectGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean | UrlTree {
    const userRole = localStorage.getItem('userRole');

    if (userRole === 'Sales') {
      return this.router.createUrlTree(['/sales']);
    } else if (userRole === 'Customer') {
      return this.router.createUrlTree(['/customer']);
    } else {
      return this.router.createUrlTree(['/login']);
    }
  }
}

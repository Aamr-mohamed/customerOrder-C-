import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const userRole = localStorage.getItem('userRole');
    const roles = route.data['roles'] as Array<string>;

    if (!userRole) {
      this.router.navigate(['/login']);
      return false;
    }
    if (roles.includes(userRole)) {
      return true;
    } else {
      if (userRole === 'Customer') {
        this.router.navigate(['/customer']);
      } else if (userRole === 'Sales') {
        this.router.navigate(['/sales']);
      }
      return false;
    }
  }
}

import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class LoginGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const userRole = localStorage.getItem('user_role');

    if (localStorage.getItem('auth_token')) {
      if (userRole === 'Admin') {
        this.router.navigate(['/admin']);
      } else if (userRole === 'Employee') {
        this.router.navigate(['/platform']);
      }

      return false;
    }

    return true;
  }
}

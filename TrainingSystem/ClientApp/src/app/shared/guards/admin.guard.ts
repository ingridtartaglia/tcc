import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AdminGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const userRole = localStorage.getItem('user_role');

    if (localStorage.getItem('auth_token') && userRole === 'Admin') {
      return true;
    }

    if (userRole === 'Employee') {
      this.router.navigate(['/platform']);
      return false;
    }

    // Se não tiver logado, redireciona pra tela de login
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}

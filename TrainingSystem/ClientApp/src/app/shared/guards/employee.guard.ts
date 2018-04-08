import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class EmployeeGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const userRole = localStorage.getItem('user_role');
    if (localStorage.getItem('auth_token') && userRole === 'Employee') {
      return true;
    }

    if (userRole === 'Admin') {
      this.router.navigate(['/admin']);
      return false;
    }

    // Se não tiver logado, redireciona pra tela de login
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}

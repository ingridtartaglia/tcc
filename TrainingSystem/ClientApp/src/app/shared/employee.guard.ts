import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class EmployeeGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let userRole = localStorage.getItem('user_role');
    if (localStorage.getItem('auth_token') && userRole == "Employee") {
      // Se estiver logado, retorna true
      return true;
    }

    if(userRole == "Admin"){
      this.router.navigate(['/admin']);
      return false;
    }

    // Senão redireciona pra tela de login
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}

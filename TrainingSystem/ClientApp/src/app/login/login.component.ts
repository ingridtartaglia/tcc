import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  email: '';
  password: '';
  loading: boolean;
  returnUrl: string;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
    // Reseta o status de login
    this.authService.logout();

    // Pega a url retornada da rota
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
  }

  login() {
    this.authService.login(this.email, this.password)
      .subscribe(
        data => {
          if (!this.returnUrl) {
            const role = localStorage.getItem('user_role');
            if (role === 'Admin') {
              this.returnUrl = '/admin';
            } else if (role === 'Employee') {
              this.returnUrl = '/platform';
            } else {
              console.log('role desconhecido: ' + role);
              this.returnUrl = '/login';
            }
          }

          this.router.navigate([this.returnUrl]);
        },
        error => {
          this.loading = false;
        }
      );
  }
}

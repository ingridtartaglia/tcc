import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../shared/services/auth.service';
import { AlertService } from '../shared/services/alert.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  email: '';
  password: '';
  returnUrl: string;
  dismissible = true;
  alerts: any[] = [];
  loading = false;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private alertService: AlertService) { }

  ngOnInit() {
    // Reseta o status de login
    this.authService.logout();

    // Pega a url retornada da rota
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];

    this.alertService.getAlert().subscribe((data) => {
      if (!data) {
        // clear alerts when an empty alert is received
        this.alerts = [];
        return;
      }

      // add alert to array
      this.alerts.push(data);
    });
  }

  login() {
    this.loading = true;
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
          this.alertService.error(error);
        }
      );
  }
}

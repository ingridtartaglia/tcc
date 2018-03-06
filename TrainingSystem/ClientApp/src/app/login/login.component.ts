import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthService } from '../core/auth.service';
import { NotificationService } from '../core/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};
  loading: boolean;
  returnUrl: string;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private notificationService: NotificationService,
    private authService: AuthService) { }

  ngOnInit() {
    // Reseta o status de login
    this.authService.logout();

    // Pega a url retornada da rota
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login() {
    this.loading = true;
    this.authService.login(this.model.email, this.model.password)
      .subscribe(
        data => {
          this.router.navigate([this.returnUrl]);
        },
        error => {
          this.notificationService.error(error);
          this.loading = false;
        });
  }

}

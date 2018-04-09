import { Component, OnInit } from '@angular/core';
import { AuthService } from './shared/services/auth.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AlertService } from './shared/services/alert.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isLoggedIn: Observable<boolean>;
  dismissible = true;
  alerts: any[];

  constructor(private router: Router,
    private authService: AuthService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn;

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

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

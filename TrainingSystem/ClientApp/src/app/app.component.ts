import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AlertService } from './shared/services/alert.service';

import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isLoggedIn: Observable<boolean>;
  dismissible = true;
  alerts: any[];

  constructor(private authService: AuthService) { }

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
}

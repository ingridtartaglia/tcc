import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../shared/services/auth.service';
import { AlertService } from '../shared/services/alert.service';

@Component({
  selector: 'app-platform',
  templateUrl: './platform.component.html',
  styleUrls: ['./platform.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class PlatformComponent implements OnInit {
  isCollapsed = true;
  dismissible = true;
  alerts: any[];

  constructor(private router: Router,
    private authService: AuthService,
    private alertService: AlertService) { }

  ngOnInit() {
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

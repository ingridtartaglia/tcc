import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  constructor(private router: Router,
    private authService: AuthService) { }

  ngOnInit() {
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}

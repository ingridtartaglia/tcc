import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isLoggedIn: Observable<boolean>;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn;
  }
}

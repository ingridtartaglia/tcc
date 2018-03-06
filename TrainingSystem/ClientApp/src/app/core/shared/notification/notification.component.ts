import { Component, OnInit } from '@angular/core';

import { NotificationService } from '../../notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  message: any;

  constructor(private notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationService.getMessage().subscribe(message => { this.message = message; });
  }
}

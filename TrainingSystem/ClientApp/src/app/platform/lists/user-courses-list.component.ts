import { Component, OnInit } from '@angular/core';
import { CourseSubscription } from '../../shared/models/course-subscription.model';
import { SubscriptionService } from '../../shared/services/subscription.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-user-courses-list',
  templateUrl: './user-courses-list.component.html',
  styleUrls: ['./user-courses-list.component.scss']
})
export class UserCoursesListComponent implements OnInit {
  userCourses: CourseSubscription[];

  constructor(private alertService: AlertService,
    private subscriptionService: SubscriptionService) { }

  ngOnInit() {
    this.getUserCoursesList();
  }

  getUserCoursesList() {
    return this.subscriptionService.getUserSubscriptions()
      .subscribe(userCourses => this.userCourses = userCourses);
  }

}

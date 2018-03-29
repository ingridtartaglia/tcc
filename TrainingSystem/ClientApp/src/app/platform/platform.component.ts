import { Component, OnInit } from '@angular/core';

import { Course } from '../shared/models/course.model';
import { CourseService } from '../shared/services/course.service';
import { SubscriptionService } from '../shared/services/subscription.service';
import { CourseSubscription } from '../shared/models/course-subscription.model';

@Component({
  selector: 'app-platform',
  templateUrl: './platform.component.html',
  styleUrls: ['./platform.component.css']
})
export class PlatformComponent implements OnInit {
  courses: Course[];
  userCourses: CourseSubscription[];

  constructor(private courseService: CourseService,
    private subscriptionService: SubscriptionService) { }

  ngOnInit() {
    this.getCoursesList();
    this.getUserCoursesList();
  }

  getCoursesList() {
    return this.courseService.getAll().subscribe(courses => this.courses = courses);
  }

  getUserCoursesList() {
    return this.subscriptionService.getUserSubscriptions()
      .subscribe(userCourses => this.userCourses = userCourses);
  }
}

import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { SubscriptionService } from '../../shared/services/subscription.service';
import { CourseSubscription } from '../../shared/models/course-subscription.model';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  courses: Course[];
  userCourses: CourseSubscription[];

  constructor(private courseService: CourseService,
    private alertService: AlertService,
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

  subscribeCourse(courseId: number) {
    this.subscriptionService.create(courseId)
      .subscribe(
      data => {
          this.getCoursesList();
          this.getUserCoursesList();
          this.alertService.success('Sua inscrição neste curso foi realizada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        });
  }
}


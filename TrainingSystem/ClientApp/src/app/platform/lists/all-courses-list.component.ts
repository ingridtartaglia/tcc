import { Component, OnInit } from '@angular/core';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { AlertService } from '../../shared/services/alert.service';
import { SubscriptionService } from '../../shared/services/subscription.service';

@Component({
  selector: 'app-all-courses-list',
  templateUrl: './all-courses-list.component.html',
  styleUrls: ['./all-courses-list.component.css']
})
export class AllCoursesListComponent implements OnInit {
  courses: Course[];

  constructor(private courseService: CourseService,
    private alertService: AlertService,
    private subscriptionService: SubscriptionService) { }

  ngOnInit() {
    this.getCoursesList();
  }

  getCoursesList() {
    return this.courseService.getAll().subscribe(courses => this.courses = courses);
  }

  subscribeCourse(courseId: number) {
    this.subscriptionService.create(courseId)
      .subscribe(
        data => {
          this.alertService.success('Sua inscrição neste curso foi realizada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        });
  }

}


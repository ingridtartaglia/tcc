import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { SubscriptionService } from '../../shared/services/subscription.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit {
  course: Course;

  constructor(private courseService: CourseService,
    private alertService: AlertService,
    private subscriptionService: SubscriptionService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getCourse();
  }

  getCourse() {
    const id = this.route.snapshot.params.courseId;
    this.courseService.getById(id).subscribe(course => this.course = course);
  }

  subscribeCourse(courseId: number) {
    this.subscriptionService.create(courseId)
      .subscribe(
        data => {
          this.getCourse();
          this.alertService.success('Sua inscrição neste curso foi realizada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        });
  }
}

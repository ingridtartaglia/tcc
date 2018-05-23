import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TagInputModule } from 'ngx-chips';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-register',
  templateUrl: './course-register.component.html',
  styleUrls: ['./course-register.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseRegisterComponent implements OnInit {
  course: Course;

  constructor(private router: Router,
    private alertService: AlertService,
    private courseService: CourseService) { }

  ngOnInit() {
    this.course = new Course();
  }

  addCourse() {
    this.courseService.create(this.course)
      .subscribe(
        data => {
          this.router.navigate([`/admin/courses/${data.courseId}`]);
          this.alertService.success('Curso criado com sucesso!'); 
        },
        error => {
          this.alertService.error(error);
        });
  }

}

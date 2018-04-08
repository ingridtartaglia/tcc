import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-course-certificate',
  templateUrl: './course-certificate.component.html',
  styleUrls: ['./course-certificate.component.css']
})
export class CourseCertificateComponent implements OnInit {
  course: Course;

  constructor(private courseService: CourseService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getCourse();
  }

  getCourse() {
    const id = this.route.snapshot.params.courseId;
    this.courseService.getById(id).subscribe(course => this.course = course);
  }

}

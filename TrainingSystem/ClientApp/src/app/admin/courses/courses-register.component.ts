import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-courses-register',
  templateUrl: './courses-register.component.html',
  styleUrls: ['./courses-register.component.css']
})
export class CoursesRegisterComponent implements OnInit {
  course: Course;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private courseService: CourseService) { }

  ngOnInit() {
    this.course = new Course();
  }

  registerCourse() {
    // this.courseService.create(this.course)
    //   .subscribe(
    //     data => {
    //       this.router.navigate(['/admin/courses/list']);
    //     },
    //     error => {
    //       console.error();
    //     });
  }

}

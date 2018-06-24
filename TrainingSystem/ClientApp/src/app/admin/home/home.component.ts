import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {
  courses: Course[];

  constructor(private courseService: CourseService) { }

  ngOnInit() {
    this.getCoursesList();
  }

  getCoursesList() {
    return this.courseService.getAll().subscribe(courses => this.courses = courses);
  }
}

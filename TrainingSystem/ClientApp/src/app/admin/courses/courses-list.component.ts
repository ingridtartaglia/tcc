import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-courses-list',
  templateUrl: './courses-list.component.html',
  styleUrls: ['./courses-list.component.css']
})
export class CoursesListComponent implements OnInit {
  courses: Course[];

  constructor(private router: Router, private courseService: CourseService) { }

  ngOnInit() {
    this.getCoursesList();
  }

  getCoursesList() {
    return this.courseService.getAll().subscribe(courses => this.courses = courses);
  }

  deleteCourse(id: number) {
    this.courseService.delete(id).subscribe(
      data => {
        this.getCoursesList();
      },
      error => {
        console.error();
      }
    );
  }

}

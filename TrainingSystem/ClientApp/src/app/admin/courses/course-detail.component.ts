import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseDetailComponent implements OnInit {
  course: Course;
  tabContent: string;

  constructor(private courseService: CourseService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.tabContent = 'lessons';
    this.getCourse();
  }

  getCourse() {
    const id = this.route.snapshot.params.courseId;
    this.courseService.getById(id).subscribe(course => this.course = course);
  }

  showTabContent(content) {
    this.tabContent = content;
  }

  updateCourseDetail() {
    this.getCourse();
  }
}

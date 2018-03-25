import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { Lesson } from '../../shared/models/lesson.model';
import { LessonService } from '../../shared/services/lesson.service';

@Component({
  selector: 'app-course-lessons',
  templateUrl: './course-lessons.component.html',
  styleUrls: ['./course-lessons.component.css']
})
export class CourseLessonsComponent implements OnInit {
  @Input() lessons: Lesson[];
  @Input() courseId: number;
  isLessonFormVisible: Boolean = false;
  newLesson: Lesson;

  constructor(private lessonService: LessonService, private router: Router) { }

  ngOnInit() {
    this.newLesson = new Lesson();
    this.newLesson.courseId = this.courseId;
  }

  showLessonForm() {
    this.isLessonFormVisible = true;
  }

  addLesson() {
    this.lessonService.create(this.newLesson)
      .subscribe(
        data => {
          this.router.navigate([`/admin/courses/${data.courseId}/lessons/${data.lessonId}`]);
        },
        error => {
          console.error();
        });
  }

  deleteLesson(id: number) {
    this.lessonService.delete(id).subscribe(
      data => {
        console.log(data);
      },
      error => {
        console.error();
      }
    );
  }
}

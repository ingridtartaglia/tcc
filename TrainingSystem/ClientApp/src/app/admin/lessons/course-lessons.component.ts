import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { Lesson } from '../../shared/models/lesson.model';
import { LessonService } from '../../shared/services/lesson.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-lessons',
  templateUrl: './course-lessons.component.html',
  styleUrls: ['./course-lessons.component.css']
})
export class CourseLessonsComponent implements OnInit {
  @Input() lessons: Lesson[];
  @Input() courseId: number;
  @Output() updateCourseDetail = new EventEmitter<any>();
  isLessonFormVisible: Boolean = false;
  newLesson: Lesson;

  constructor(private lessonService: LessonService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newLesson = new Lesson(this.courseId);
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
          this.alertService.error(error);
        });
  }

  deleteLesson(id: number) {
    this.lessonService.delete(id).subscribe(
      data => {
        this.updateCourseDetail.emit();
        this.alertService.success('Unidade deletada com sucesso!');
      },
      error => {
        this.alertService.error(error);
      }
    );
  }
}

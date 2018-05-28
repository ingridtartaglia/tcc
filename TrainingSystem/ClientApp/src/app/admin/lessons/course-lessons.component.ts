import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { Lesson } from '../../shared/models/lesson.model';
import { LessonService } from '../../shared/services/lesson.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-lessons',
  templateUrl: './course-lessons.component.html',
  styleUrls: ['./course-lessons.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseLessonsComponent implements OnInit {
  @Input() lessons: Lesson[];
  @Input() courseId: number;
  @Output() updateCourseDetail = new EventEmitter<any>();
  isLessonListVisible: Boolean = true;
  isLessonFormVisible: Boolean = false;
  newLesson: Lesson;

  constructor(private lessonService: LessonService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newLesson = new Lesson(this.courseId);
  }

  showLessonForm() {
    this.isLessonListVisible = false;
    this.isLessonFormVisible = true;
    this.newLesson.name = null;
  }

  backToLessonList() {
    this.isLessonListVisible = true;
    this.isLessonFormVisible = false;
  }

  addLesson() {
    this.lessonService.create(this.newLesson)
      .subscribe(
        data => {
          this.updateCourseDetail.emit();
          this.backToLessonList();
          this.alertService.success('Unidade criada com sucesso!');
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

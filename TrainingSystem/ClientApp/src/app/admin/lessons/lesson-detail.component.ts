import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Video } from '../../shared/models/video.model';
import { Lesson } from '../../shared/models/lesson.model';
import { LessonService } from '../../shared/services/lesson.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-lesson-detail',
  templateUrl: './lesson-detail.component.html',
  styleUrls: ['./lesson-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonDetailComponent implements OnInit {
  lesson: Lesson;
  tabContent: string;

  constructor(private route: ActivatedRoute,
    private lessonService: LessonService,
    private router: Router,
    private alertService: AlertService) { }

  ngOnInit() {
    this.tabContent = 'videos';
    this.getLesson();
  }

  getLesson() {
    const id = this.route.snapshot.params.lessonId;
    this.lessonService.getById(id).subscribe(lesson => this.lesson = lesson);
  }

  showTabContent(content) {
    this.tabContent = content;
  }

  updateLessonDetail() {
    this.getLesson();
  }

  deleteLesson(id: number) {
    this.lessonService.delete(id).subscribe(
      data => {
        this.router.navigate([`/admin/courses/${this.lesson.courseId}`]);
        this.alertService.success('Unidade deletada com sucesso!');
      },
      error => {
        this.alertService.error(error);
      }
    );
  }
}

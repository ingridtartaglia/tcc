import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Lesson } from '../../../shared/models/lesson.model';
import { Video } from '../../../shared/models/video.model';
import { LessonService } from '../../../shared/services/lesson.service';

@Component({
  selector: 'app-lesson-detail',
  templateUrl: './lesson-detail.component.html',
  styleUrls: ['./lesson-detail.component.css']
})
export class LessonDetailComponent implements OnInit {
  lesson: Lesson;

  constructor(private route: ActivatedRoute,
    private lessonService: LessonService) { }

  ngOnInit() {
    this.getLesson();
  }

  getLesson() {
    const id = this.route.snapshot.params.lessonId;
    this.lessonService.getById(id).subscribe(lesson => this.lesson = lesson);
  }
}

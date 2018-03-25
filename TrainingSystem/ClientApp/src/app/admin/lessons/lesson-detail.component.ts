import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Video } from '../../shared/models/video.model';
import { Lesson } from '../../shared/models/lesson.model';
import { LessonService } from '../../shared/services/lesson.service';

@Component({
  selector: 'app-lesson-detail',
  templateUrl: './lesson-detail.component.html',
  styleUrls: ['./lesson-detail.component.css']
})
export class LessonDetailComponent implements OnInit {
  lesson: Lesson;
  tabContent: string;

  constructor(private route: ActivatedRoute,
    private lessonService: LessonService) { }

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
}

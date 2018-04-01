import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VgAPI } from 'videogular2/core';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { Video } from '../../shared/models/video.model';

@Component({
  selector: 'app-course-video',
  templateUrl: './course-video.component.html',
  styleUrls: ['./course-video.component.css']
})
export class CourseVideoComponent {
  api: VgAPI;
  course: Course;
  currentVideo: Video;
  isSidebarCollapsed: Boolean = true;
  videos: object = {};

  constructor(private courseService: CourseService, private route: ActivatedRoute) {
    route.params.subscribe(val => {
      if (!this.course) {
        this.getCourse();
      } else {
        const videoId = this.route.snapshot.params.videoId;
        this.setCurrentVideo(videoId);
      }
    });
  }

  getCourse() {
    const courseId = this.route.snapshot.params.courseId;
    return this.courseService.getById(courseId).subscribe(course => {
      this.course = course;

      this.course.lessons.forEach(lesson =>
        lesson.videos.forEach(video =>
          this.videos[video.videoId] = video
        )
      );

      const videoId = this.route.snapshot.params.videoId;
      this.setCurrentVideo(videoId);
    });
  }

  setCurrentVideo(videoId: number) {
    this.currentVideo = this.videos[videoId];
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;

    // this.api.getDefaultMedia().subscriptions.ended.subscribe(this.nextVideo.bind(this));
  }
}

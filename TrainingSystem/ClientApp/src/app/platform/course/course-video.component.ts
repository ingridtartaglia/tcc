import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VgAPI } from 'videogular2/core';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';
import { Video } from '../../shared/models/video.model';
import { VideoWatchService } from '../../shared/services/video-watch.service';

@Component({
  selector: 'app-course-video',
  templateUrl: './course-video.component.html',
  styleUrls: ['./course-video.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CourseVideoComponent {
  api: VgAPI;
  course: Course;
  currentVideo: Video;
  isSidebarCollapsed: Boolean = window.innerWidth > 992 ? false : true;
  videos: object = {};
  showCompletedMessage: Boolean = false;

  constructor(private courseService: CourseService,
              private videoWatchService: VideoWatchService,
    private route: ActivatedRoute) {
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

      this.course.videoWatch.forEach(videoWatch => {
        this.videos[videoWatch.videoId].videoWatch = videoWatch;
      });

      const videoId = this.route.snapshot.params.videoId;
      this.setCurrentVideo(videoId);
    });
  }

  setCurrentVideo(videoId: number) {
    this.currentVideo = this.videos[videoId];
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;

    this.api.getDefaultMedia().subscriptions.playing.subscribe(
      data => {
        if (!this.currentVideo.videoWatch) {
          this.videoWatchService.create(this.currentVideo.videoId)
            .subscribe(vw => {
              this.currentVideo.videoWatch = vw;
            });
        }
      });
    this.api.getDefaultMedia().subscriptions.ended.subscribe(
      data => {
        this.currentVideo.videoWatch.isCompleted = true;
        this.videoWatchService.update(this.currentVideo.videoWatch)
          .subscribe(vw => {
            this.showCompletedMessage = true;
          });
      });
  }
}

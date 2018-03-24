import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { Video } from '../../shared/models/video.model';
import { VideoService } from '../../shared/services/video.service';

@Component({
  selector: 'app-lesson-videos',
  templateUrl: './lesson-videos.component.html',
  styleUrls: ['./lesson-videos.component.css']
})
export class LessonVideosComponent implements OnInit {
  @Input() videos: Video[];
  @Input() lessonId: number;
  isVideoFormVisible: Boolean = false;
  newVideo: Video;

  constructor(private videoService: VideoService, private router: Router) { }

  ngOnInit() {
    this.newVideo = new Video();
    this.newVideo.lessonId = this.lessonId;
  }

  showVideoForm() {
    this.isVideoFormVisible = true;
  }

  addVideo() {
    this.videoService.create(this.newVideo)
      .subscribe(
        data => {
          console.log('sucesso');
        },
        error => {
          console.error();
        }
      );
  }

  deleteVideo(id: number) {
    this.videoService.delete(id)
      .subscribe(
        data => {
          console.log('sucesso');
        },
        error => {
          console.error();
        }
      );
  }

  fileChange(files: FileList) {
    if (files && files[0].size > 0) {
      this.newVideo.file = files[0];
    }
  }
}

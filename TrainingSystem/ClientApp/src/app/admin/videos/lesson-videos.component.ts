import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { Video } from '../../shared/models/video.model';
import { VideoService } from '../../shared/services/video.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-lesson-videos',
  templateUrl: './lesson-videos.component.html',
  styleUrls: ['./lesson-videos.component.css']
})
export class LessonVideosComponent implements OnInit {
  @Input() videos: Video[];
  @Input() lessonId: number;
  @Output() updateLessonDetail = new EventEmitter<any>();
  isVideoFormVisible: Boolean = false;
  newVideo: Video;
  fileSelected: Boolean = false;
  isFileTypeSupported: Boolean = false;

  constructor(private videoService: VideoService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newVideo = new Video(this.lessonId);
  }

  showVideoForm() {
    this.isVideoFormVisible = true;
  }

  addVideo() {
    this.videoService.create(this.newVideo)
      .subscribe(
        data => {
          this.newVideo = new Video(this.lessonId);
          this.isVideoFormVisible = false;
          this.updateLessonDetail.emit();
          this.alertService.success('Vídeo adicionado com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  deleteVideo(id: number) {
    this.videoService.delete(id)
      .subscribe(
        data => {
          this.updateLessonDetail.emit();
          this.alertService.success('Vídeo deletado com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  fileChange(files: FileList) {
    if (files && files[0].size > 0) {
      if (files[0].type === 'video/mp4'
        || files[0].type === 'video/x-m4v'
        || files[0].type === 'video/*') {
        this.isFileTypeSupported = true;
      } else {
        this.isFileTypeSupported = false;
      }

      this.newVideo.file = files[0];
      this.fileSelected = true;
    }
  }
}

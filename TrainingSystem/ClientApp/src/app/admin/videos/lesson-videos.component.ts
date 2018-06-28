import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { Video } from '../../shared/models/video.model';
import { VideoService } from '../../shared/services/video.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-lesson-videos',
  templateUrl: './lesson-videos.component.html',
  styleUrls: ['./lesson-videos.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonVideosComponent implements OnInit {
  @Input() videos: Video[];
  @Input() lessonId: number;
  @Output() updateLessonDetail = new EventEmitter<any>();
  isVideoListVisible: Boolean = true;
  isVideoFormVisible: Boolean = false;
  newVideo: Video;
  fileSelected: Boolean = false;
  isFileTypeSupported: Boolean = false;
  loading = false;

  constructor(private videoService: VideoService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newVideo = new Video(this.lessonId);
  }

  showVideoForm() {
    this.isVideoFormVisible = true;
    this.isVideoListVisible = false;
  }

  backToVideoList() {
    this.isVideoListVisible = true;
    this.isVideoFormVisible = false;
  }

  addVideo() {
    this.loading = true;
    this.videoService.create(this.newVideo)
      .subscribe(
        data => {
          this.newVideo = new Video(this.lessonId);
          this.updateLessonDetail.emit();
          this.fileSelected = false;
          this.backToVideoList();
          this.alertService.success('Vídeo adicionado com sucesso!');
          this.loading = false;
        },
        error => {
          this.loading = false;
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
      if ((files[0].type === 'video/mp4'
        || files[0].type === 'video/x-m4v'
        || files[0].type === 'video/*') && files[0].size < 31457280) {
        this.isFileTypeSupported = true;
      } else {
        this.isFileTypeSupported = false;
      }

      this.newVideo.file = files[0];
      this.fileSelected = true;
    }
  }
}

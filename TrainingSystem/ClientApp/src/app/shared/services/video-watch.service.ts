import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Video } from '../models/video.model';
import { VideoWatch } from '../models/video-watch.model';

@Injectable()
export class VideoWatchService {
  constructor(private http: HttpClient) { }

  create(videoId: number): Observable<any> {
    return this.http.post('/api/VideoWatch', videoId);
  }

  update(video: VideoWatch): Observable<any> {
    return this.http.put(`/api/VideoWatch`, video);
  }

}

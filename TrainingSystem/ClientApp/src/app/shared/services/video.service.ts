import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Video } from '../models/video.model';

@Injectable()
export class VideoService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Video[]> {
    return this.http.get<Video[]>('/api/Videos');
  }

  getById(videoId: number): Observable<Video> {
    return this.http.get<Video>(`/api/Lessons/${videoId}`);
  }

  create(video: Video): Observable<any> {
    return this.http.post('/api/Lessons', video);
  }
}

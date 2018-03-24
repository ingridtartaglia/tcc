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
    return this.http.get<Video>(`/api/Videos/${videoId}`);
  }

  create(video: Video): Observable<any> {
    const formData = new FormData();
    formData.append('name', video.name);
    formData.append('file', video.file);
    formData.append('lessonId', video.lessonId.toString());

    return this.http.post('/api/Videos', formData);
  }
  delete(videoId: number): Observable<any> {
    return this.http.delete(`/api/Videos/${videoId}`);
  }
}

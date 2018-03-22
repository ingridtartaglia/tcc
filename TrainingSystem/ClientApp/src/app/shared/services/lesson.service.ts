import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Lesson } from '../models/lesson.model';

@Injectable()
export class LessonService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Lesson[]> {
    return this.http.get<Lesson[]>('/api/Lessons');
  }

  getById(lessonId: number): Observable<Lesson> {
    return this.http.get<Lesson>(`/api/Lessons/${lessonId}`);
  }

  create(lesson: Lesson): Observable<any> {
    return this.http.post('/api/Lessons', lesson);
  }

  delete(lessonId: number): Observable<any> {
    return this.http.delete(`/api/Lessons/${lessonId}`);
  }

}

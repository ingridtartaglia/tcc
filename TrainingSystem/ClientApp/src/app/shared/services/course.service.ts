import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Course } from '../models/course.model';

@Injectable()
export class CourseService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Course[]> {
    return this.http.get<Course[]>('/api/Courses');
  }

  getById(courseId: number): Observable<Course> {
    return this.http.get<Course>(`/api/Courses/${courseId}`);
  }

  create(course: Course): Observable<any> {
    return this.http.post('/api/Courses', course);
  }

}

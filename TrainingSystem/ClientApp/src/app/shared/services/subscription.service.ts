import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { CourseSubscription } from '../models/course-subscription.model';

@Injectable()
export class SubscriptionService {

  constructor(private http: HttpClient) { }

  getUserSubscriptions(): Observable<CourseSubscription[]> {
    return this.http.get<CourseSubscription[]>('/api/Subscription/UserSubscriptions');
  }

  getCourseSubscriptions(courseId: number): Observable<CourseSubscription[]> {
    return this.http.get<CourseSubscription[]>('/api/Subscription/CourseSubscriptions');
  }

  create(courseSubscription: CourseSubscription): Observable<any> {
    return this.http.post('/api/Subscription/CourseSubscriptions', courseSubscription);
  }

  delete(courseId: number): Observable<any> {
    return this.http.delete(`/api/Subscription/CourseSubscriptions/${courseId}`);
  }

}

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';

import { Rating } from '../models/rating.model';

@Injectable()
export class RatingService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Rating[]> {
    return this.http.get<Rating[]>('/api/Ratings');
  }

  getById(ratingId: number): Observable<Rating> {
    return this.http.get<Rating>(`/api/Ratings/${ratingId}`);
  }

  create(rating: Rating): Observable<any> {
    return this.http.post('/api/Ratings', rating);
  }

  delete(ratingId: number): Observable<any> {
    return this.http.delete(`/api/Ratings/${ratingId}`);
  }
}

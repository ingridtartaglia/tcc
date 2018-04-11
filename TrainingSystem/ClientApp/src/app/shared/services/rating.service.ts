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

  create(rating: Rating): Observable<any> {
    return this.http.post('/api/Ratings', rating);
  }

}

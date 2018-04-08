import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Question } from '../models/question.model';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Question[]> {
    return this.http.get<Question[]>('/api/Questions');
  }

  getById(questionId: number): Observable<Question> {
    return this.http.get<Question>(`/api/Questions/${questionId}`);
  }

  create(question: Question): Observable<any> {
    return this.http.post('/api/Questions', question);
  }

  delete(questionId: number): Observable<any> {
    return this.http.delete(`/api/Questions/${questionId}`);
  }
}

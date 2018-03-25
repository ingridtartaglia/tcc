import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Exam } from '../models/exam.model';

@Injectable()
export class ExamService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Exam[]> {
    return this.http.get<Exam[]>('/api/Exams');
  }

  getById(examId: number): Observable<Exam> {
    return this.http.get<Exam>(`/api/Exams/${examId}`);
  }

  create(exam: Exam): Observable<any> {
    return this.http.post('/api/Exams', exam);
  }

  delete(examId: number): Observable<any> {
    return this.http.delete(`/api/Exams/${examId}`);
  }
}

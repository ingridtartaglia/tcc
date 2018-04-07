import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { UserExam } from '../models/user-exam.model';

@Injectable()
export class UserExamService {
  constructor(private http: HttpClient) { }

  getAll(): Observable<UserExam[]> {
    return this.http.get<UserExam[]>('/api/UserExams');
  }

  getById(userExamId: number): Observable<UserExam> {
    return this.http.get<UserExam>(`/api/UserExams/${userExamId}`);
  }

  create(userExam: UserExam): Observable<any> {
    return this.http.post('/api/UserExams', userExam);
  }

}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { UserExam } from '../models/user-exam.model';

@Injectable()
export class UserExamService {
  constructor(private http: HttpClient) { }

  isApproved(examId: number): Observable<boolean> {
    return this.http.get<boolean>(`/api/UserExams/${examId}`);
  }

  create(userExam: UserExam): Observable<any> {
    return this.http.post('/api/UserExams', userExam);
  }

}

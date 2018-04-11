import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { UserExam } from '../models/user-exam.model';

@Injectable()
export class UserExamService {
  constructor(private http: HttpClient) { }

  create(userExam: UserExam): Observable<any> {
    return this.http.post('/api/UserExams', userExam);
  }

}

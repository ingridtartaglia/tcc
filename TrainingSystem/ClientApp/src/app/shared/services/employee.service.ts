import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Employee } from './../models/employee.model';
import { Register } from './../models/register.model';

@Injectable()
export class EmployeeService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>('/api/Employees');
  }

  getById(employeeId: number): Observable<Employee> {
    return this.http.get<Employee>(`/api/Employees/${employeeId}`);
  }

  create(register: Register): Observable<any> {
    return this.http.post('/api/Employees', register);
  }
}

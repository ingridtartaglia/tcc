import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './employee.model';
import { Register } from './register.model';

@Injectable()
export class EmployeeService {
  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<Employee[]>('/api/Employees');
  }

  getById(id: number) {
    return this.http.get('/api/Employees/' + id);
  }

  create(register: Register) {
    return this.http.post('/api/Employees', register);
  }

  delete(id: number) {
    return this.http.delete('/api/Employees' + id);
  }
}

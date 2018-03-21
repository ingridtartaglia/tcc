import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Register } from '../../shared/models/register.model';
import { EmployeeService } from '../../shared/services/employee.service';

@Component({
  selector: 'app-employees-register',
  templateUrl: './employees-register.component.html',
  styleUrls: ['./employees-register.component.css']
})
export class EmployeesRegisterComponent implements OnInit {
  register: Register;

  constructor(private router: Router,
    private employeeService: EmployeeService) { }

  ngOnInit() {
    this.register = new Register();
  }

  registerEmployees() {
    this.employeeService.create(this.register)
      .subscribe(
        data => {
          this.router.navigate(['/admin/employees/list']);
        },
        error => {
          console.error();
        });
  }

}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Employee } from '../../shared/models/employee.model';
import { EmployeeService } from '../../shared/services/employee.service';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesListComponent implements OnInit {
  employees: Employee[];

  constructor(private router: Router,
    private employeeService: EmployeeService) { }

  ngOnInit() {
    this.getEmployeesList();
  }

  getEmployeesList() {
    this.employeeService.getAll().subscribe(employees => this.employees = employees);
  }

}

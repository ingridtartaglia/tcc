import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Employee } from '../../shared/models/employee.model';
import { EmployeeService } from '../../shared/services/employee.service';

@Component({
  selector: 'app-employees-detail',
  templateUrl: './employees-detail.component.html',
  styleUrls: ['./employees-detail.component.css']
})
export class EmployeesDetailComponent implements OnInit {
  employee: Employee;

  constructor(private employeeService: EmployeeService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.getEmployee();
  }

  getEmployee() {
    const id = this.route.snapshot.params.employeeId;
    this.employeeService.getById(id).subscribe(employee => this.employee = employee);
  }

}

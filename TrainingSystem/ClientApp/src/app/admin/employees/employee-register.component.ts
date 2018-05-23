import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { Register } from '../../shared/models/register.model';
import { EmployeeService } from '../../shared/services/employee.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-employee-register',
  templateUrl: './employee-register.component.html',
  styleUrls: ['./employee-register.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EmployeeRegisterComponent implements OnInit {
  register: Register;

  constructor(private router: Router,
    private alertService: AlertService,
    private employeeService: EmployeeService) { }

  ngOnInit() {
    this.register = new Register();
  }

  registerEmployees() {
    this.employeeService.create(this.register)
      .subscribe(
        data => {
          this.router.navigate([`/admin/employees/${data.employeeId}`]);
          this.alertService.success('Usuário criado com sucesso!');
        },
        error => {
          this.alertService.error(error);
        });
  }

}

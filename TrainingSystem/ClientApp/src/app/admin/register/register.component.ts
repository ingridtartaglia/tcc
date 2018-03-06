import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { EmployeeService } from './shared/employee.service';
import { Register } from './shared/register.model';
import { NotificationService } from '../../core/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model: Register = new Register();
  loading = false;

  constructor(
    private router: Router,
    private notificationService: NotificationService,
    private employeeService: EmployeeService) { }

  register() {
    this.loading = true;
    this.employeeService.create(this.model)
      .subscribe(
        data => {
          this.notificationService.success('Cadastro criado com sucesso!', true);
          this.router.navigate(['/login']);
        },
        error => {
          this.notificationService.error(error);
          this.loading = false;
        });
  }
}

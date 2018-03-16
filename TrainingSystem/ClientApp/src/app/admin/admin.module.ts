import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { HomeComponent } from './home/home.component';
import { EmployeesListComponent } from './employees/employees-list.component';
import { EmployeesRegisterComponent } from './employees/employees-register.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule
  ],
  declarations: [
    HomeComponent,
    AdminComponent,
    EmployeesListComponent,
    EmployeesRegisterComponent
  ]
})
export class AdminModule { }

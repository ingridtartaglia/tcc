import { NgModule } from '@angular/core';
import { TagInputModule } from 'ngx-chips';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { HomeComponent } from './home/home.component';
import { EmployeesListComponent } from './employees/employees-list.component';
import { EmployeesRegisterComponent } from './employees/employees-register.component';
import { EmployeesDetailComponent } from './employees/employees-detail.component';
import { CoursesRegisterComponent } from './courses/courses-register.component';
import { CoursesListComponent } from './courses/courses-list.component';

@NgModule({
  imports: [
    CommonModule,
    TagInputModule,
    ReactiveFormsModule,
    FormsModule,
    AdminRoutingModule
  ],
  declarations: [
    HomeComponent,
    AdminComponent,
    EmployeesListComponent,
    EmployeesRegisterComponent,
    EmployeesDetailComponent,
    CoursesRegisterComponent,
    CoursesListComponent
  ]
})
export class AdminModule { }

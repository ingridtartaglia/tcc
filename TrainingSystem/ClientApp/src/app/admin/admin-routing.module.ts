import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './admin.component';
import { HomeComponent } from './home/home.component';
import { EmployeesListComponent } from './employees/employees-list.component';
import { EmployeesRegisterComponent } from './employees/employees-register.component';
import { EmployeesDetailComponent } from './employees/employees-detail.component';
import { CoursesListComponent } from './courses/courses-list.component';
import { CoursesRegisterComponent } from './courses/courses-register.component';
import { CoursesDetailComponent } from './courses/courses-detail.component';
import { LessonDetailComponent } from './courses/lessons/lesson-detail.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'employees/list', component: EmployeesListComponent },
      { path: 'employees/add', component: EmployeesRegisterComponent },
      { path: 'employees/:employeeId', component: EmployeesDetailComponent },
      { path: 'courses/list', component: CoursesListComponent },
      { path: 'courses/add', component: CoursesRegisterComponent },
      { path: 'courses/:courseId', component: CoursesDetailComponent },
      { path: 'courses/:courseId/lessons/:lessonId', component: LessonDetailComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

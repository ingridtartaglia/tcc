import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './admin.component';
import { HomeComponent } from './home/home.component';
import { EmployeesListComponent } from './employees/employees-list.component';
import { EmployeeRegisterComponent } from './employees/employee-register.component';
import { EmployeeDetailComponent } from './employees/employee-detail.component';
import { CoursesListComponent } from './courses/courses-list.component';
import { CourseRegisterComponent } from './courses/course-register.component';
import { CourseDetailComponent } from './courses/course-detail.component';
import { LessonDetailComponent } from './lessons/lesson-detail.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'employees/list', component: EmployeesListComponent },
      { path: 'employees/add', component: EmployeeRegisterComponent },
      { path: 'employees/:employeeId', component: EmployeeDetailComponent },
      { path: 'courses/list', component: CoursesListComponent },
      { path: 'courses/add', component: CourseRegisterComponent },
      { path: 'courses/:courseId', component: CourseDetailComponent },
      { path: 'courses/:courseId/lessons/:lessonId', component: LessonDetailComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

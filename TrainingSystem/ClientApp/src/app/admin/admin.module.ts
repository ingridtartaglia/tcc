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
import { CoursesDetailComponent } from './courses/courses-detail.component';
import { LessonDetailComponent } from './courses/lessons/lesson-detail.component';
import { CourseLessonsComponent } from './courses/lessons/course-lessons.component';
import { LessonVideosComponent } from './courses/lessons/lesson-videos.component';

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
    CoursesListComponent,
    CoursesDetailComponent,
    LessonDetailComponent,
    CourseLessonsComponent,
    LessonVideosComponent
  ]
})
export class AdminModule { }

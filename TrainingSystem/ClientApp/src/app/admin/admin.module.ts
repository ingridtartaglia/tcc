import { NgModule } from '@angular/core';
import { TagInputModule } from 'ngx-chips';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccordionModule } from 'ngx-bootstrap';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { HomeComponent } from './home/home.component';
import { EmployeesListComponent } from './employees/employees-list.component';
import { EmployeeRegisterComponent } from './employees/employee-register.component';
import { EmployeeDetailComponent } from './employees/employee-detail.component';
import { CoursesListComponent } from './courses/courses-list.component';
import { CourseRegisterComponent } from './courses/course-register.component';
import { CourseDetailComponent } from './courses/course-detail.component';
import { CourseLessonsComponent } from './lessons/course-lessons.component';
import { LessonDetailComponent } from './lessons/lesson-detail.component';
import { LessonVideosComponent } from './videos/lesson-videos.component';
import { LessonExamComponent } from './exams/lesson-exam.component';
import { CourseMaterialsComponent } from './materials/course-materials.component';

@NgModule({
  imports: [
    CommonModule,
    TagInputModule,
    ReactiveFormsModule,
    FormsModule,
    AdminRoutingModule,
    AccordionModule.forRoot()
  ],
  declarations: [
    HomeComponent,
    AdminComponent,
    EmployeesListComponent,
    EmployeeRegisterComponent,
    EmployeeDetailComponent,
    CoursesListComponent,
    CourseRegisterComponent,
    CourseDetailComponent,
    CourseLessonsComponent,
    LessonDetailComponent,
    LessonVideosComponent,
    LessonExamComponent,
    CourseMaterialsComponent
  ]
})
export class AdminModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CourseComponent } from './course/course.component';
import { CourseVideoComponent } from './course/course-video.component';
import { CourseExamComponent } from './course/course-exam.component';
import { CourseCertificateComponent } from './course/course-certificate.component';
import { AllCoursesListComponent } from './lists/all-courses-list.component';
import { UserCoursesListComponent } from './lists/user-courses-list.component';

const routes: Routes = [
  {
    path: '',
    component: PlatformComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'courses/list', component: AllCoursesListComponent },
      { path: 'courses/your-list', component: UserCoursesListComponent },
      { path: 'courses/:courseId', component: CourseComponent },
      { path: 'courses/:courseId/videos/:videoId', component: CourseVideoComponent },
      { path: 'courses/:courseId/exams/:examId', component: CourseExamComponent },
      { path: 'courses/:courseId/certificate', component: CourseCertificateComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlatformRoutingModule { }

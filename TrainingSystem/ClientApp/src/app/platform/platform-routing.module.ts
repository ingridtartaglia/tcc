import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CoursesComponent } from './courses/courses.component';
import { CourseVideoComponent } from './courses/course-video.component';
import { CourseExamComponent } from './courses/course-exam.component';
import { CourseCertificateComponent } from './courses/course-certificate.component';

const routes: Routes = [
  {
    path: '',
    component: PlatformComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'courses/:courseId', component: CoursesComponent },
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

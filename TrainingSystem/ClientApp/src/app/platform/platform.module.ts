import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccordionModule, RatingModule, TabsModule, CollapseModule, AlertModule } from 'ngx-bootstrap';
import { VgCoreModule } from 'videogular2/core';
import { VgControlsModule } from 'videogular2/controls';
import { VgOverlayPlayModule } from 'videogular2/overlay-play';
import { VgBufferingModule } from 'videogular2/buffering';

import { PlatformRoutingModule } from './platform-routing.module';
import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CourseComponent } from './course/course.component';
import { CourseRatingComponent } from './course/course-rating.component';
import { CourseVideoComponent } from './course/course-video.component';
import { CourseExamComponent } from './course/course-exam.component';
import { CourseCertificateComponent } from './course/course-certificate.component';
import { AllCoursesListComponent } from './lists/all-courses-list.component';
import { UserCoursesListComponent } from './lists/user-courses-list.component';

@NgModule({
  imports: [
    CommonModule,
    PlatformRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AccordionModule.forRoot(),
    RatingModule.forRoot(),
    TabsModule.forRoot(),
    CollapseModule.forRoot(),
    AlertModule.forRoot(),
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule
  ],
  declarations: [
    PlatformComponent,
    HomeComponent,
    CourseComponent,
    CourseRatingComponent,
    CourseVideoComponent,
    CourseExamComponent,
    CourseCertificateComponent,
    AllCoursesListComponent,
    UserCoursesListComponent
  ]
})
export class PlatformModule { }

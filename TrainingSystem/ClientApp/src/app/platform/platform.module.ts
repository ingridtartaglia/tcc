import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccordionModule, RatingModule, TabsModule, CollapseModule } from 'ngx-bootstrap';
import { VgCoreModule } from 'videogular2/core';
import { VgControlsModule } from 'videogular2/controls';
import { VgOverlayPlayModule } from 'videogular2/overlay-play';
import { VgBufferingModule } from 'videogular2/buffering';

import { PlatformRoutingModule } from './platform-routing.module';
import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CoursesComponent } from './courses/courses.component';
import { CourseRatingComponent } from './courses/course-rating.component';
import { CourseVideoComponent } from './courses/course-video.component';

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
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule
  ],
  declarations: [
    PlatformComponent,
    HomeComponent,
    CoursesComponent,
    CourseRatingComponent,
    CourseVideoComponent
  ]
})
export class PlatformModule { }

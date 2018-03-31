import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccordionModule, RatingModule, TabsModule } from 'ngx-bootstrap';

import { PlatformRoutingModule } from './platform-routing.module';
import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CoursesComponent } from './courses/courses.component';
import { CourseRatingComponent } from './courses/course-rating.component';

@NgModule({
  imports: [
    CommonModule,
    PlatformRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AccordionModule.forRoot(),
    RatingModule.forRoot(),
    TabsModule.forRoot(),
  ],
  declarations: [
    PlatformComponent,
    HomeComponent,
    CoursesComponent,
    CourseRatingComponent
  ]
})
export class PlatformModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccordionModule } from 'ngx-bootstrap';

import { PlatformRoutingModule } from './platform-routing.module';
import { PlatformComponent } from './platform.component';
import { HomeComponent } from './home/home.component';
import { CoursesComponent } from './courses/courses.component';

@NgModule({
  imports: [
    CommonModule,
    PlatformRoutingModule,
    AccordionModule.forRoot()
  ],
  declarations: [
    PlatformComponent,
    HomeComponent,
    CoursesComponent
  ]
})
export class PlatformModule { }

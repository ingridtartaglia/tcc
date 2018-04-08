import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthService } from './shared/services/auth.service';
import { EmployeeService } from './shared/services/employee.service';
import { JwtInterceptor } from './shared/jwt.interceptor';
import { AuthGuard } from './shared/guards/auth.guard';
import { AdminGuard } from './shared/guards/admin.guard';
import { EmployeeGuard } from './shared/guards/employee.guard';
import { CourseService } from './shared/services/course.service';
import { LessonService } from './shared/services/lesson.service';
import { VideoService } from './shared/services/video.service';
import { ExamService } from './shared/services/exam.service';
import { MaterialService } from './shared/services/material.service';
import { QuestionService } from './shared/services/question.service';
import { SubscriptionService } from './shared/services/subscription.service';
import { RatingService } from './shared/services/rating.service';
import { VideoWatchService } from './shared/services/video-watch.service';
import { UserExamService } from './shared/services/user-exam.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    AuthGuard,
    AdminGuard,
    EmployeeGuard,
    AuthService,
    EmployeeService,
    CourseService,
    LessonService,
    VideoService,
    ExamService,
    MaterialService,
    QuestionService,
    SubscriptionService,
    RatingService,
    VideoWatchService,
    UserExamService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

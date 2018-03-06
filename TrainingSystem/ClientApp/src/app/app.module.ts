import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './admin/home/home.component';

import { AuthGuard } from './core/auth.guard';
import { AuthService } from './core/auth.service';
import { JwtInterceptor } from './core/jwt.interceptor';
import { EmployeeService } from './admin/register/shared/employee.service';

import { AppRoutingModule } from './app-routing.module';
import { RegisterComponent } from './admin/register/register.component';
import { NotificationComponent } from './core/shared/notification/notification.component';
import { NotificationService } from './core/notification.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    NotificationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    AuthGuard,
    AuthService,
    EmployeeService,
    NotificationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

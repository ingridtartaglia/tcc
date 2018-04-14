import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Employee } from '../../shared/models/employee.model';
import { EmployeeService } from '../../shared/services/employee.service';
import { CourseService } from '../../shared/services/course.service';
import { Course } from '../../shared/models/course.model';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss']
})
export class EmployeeDetailComponent implements OnInit {
  employee: Employee;
  courses: Course[];

  constructor(private employeeService: EmployeeService, private courseService: CourseService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getEmployee();
  }

  getEmployee() {
    const id = this.route.snapshot.params.employeeId;
    this.employeeService.getById(id).subscribe(employee => this.employee = employee);
    this.courseService.getUserCourses(id).subscribe(courses => this.courses = courses);
  }

  getUserStatusInCourse(course: Course) {
    if (course.isCompleted) {
      return 'Aprovado';
    } else if (course.approvedExamsPercentage < 70 && course.watchedVideosPercentage === 100) {
      return 'Reprovado';
    } else if (course.watchedVideosPercentage !== 100) {
      return 'Cursando';
    }
  }

}

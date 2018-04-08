import { Course } from './course.model';
import { Employee } from './employee.model';

export class CourseSubscription {
  courseId: number;
  course: Course;
  employeeId: number;
  employee: Employee;
}

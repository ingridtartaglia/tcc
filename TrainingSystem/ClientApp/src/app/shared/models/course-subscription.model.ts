import { Course } from './course.model';
import { Employee } from './employee.model';

export class CourseSubscription {
  courseId: number;
  courses: Course[];
  employeeId: number;
  employees: Employee[];
}

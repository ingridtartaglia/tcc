import { Video } from './video.model';
import { Employee } from './employee.model';

export class VideoWatch {
  videoId: number;
  video: Video;
  employeeId: number;
  employee: Employee;
  isCompleted: boolean;
}

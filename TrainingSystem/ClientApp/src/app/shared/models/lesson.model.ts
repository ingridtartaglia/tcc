import { Exam } from './exam.model';
import { Video } from './video.model';

export class Lesson {
  constructor(courseId: number) {
    this.courseId = courseId;
  }
  lessonId: number;
  courseId: number;
  name: string;
  exam: Exam;
  videos: Video[];
}

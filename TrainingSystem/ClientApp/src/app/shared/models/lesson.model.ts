import { Exam } from './exam.model';
import { Video } from './video.model';

export class Lesson {
  lessonId: number;
  name: string;
  exam: Exam;
  videos: Video;
}

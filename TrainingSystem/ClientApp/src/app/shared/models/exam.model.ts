import { Question } from './question.model';
import { Lesson } from './lesson.model';

export class Exam {
  constructor(lessonId: number) {
    this.lessonId = lessonId;
  }
  lessonId: number;
  examId: number;
  lesson: Lesson;
  questions: Question[];
}

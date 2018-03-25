import { Question } from './question.model';

export class Exam {
  constructor(lessonId: number) {
    this.lessonId = lessonId;
  }
  lessonId: number;
  examId: number;
  questions: Question[];
}

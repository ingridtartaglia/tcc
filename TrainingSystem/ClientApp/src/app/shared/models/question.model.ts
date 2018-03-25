import { QuestionChoice } from './question-choice.model';

export class Question {
  constructor(examId: number) {
    this.examId = examId;
  }
  examId: number;
  questionId: number;
  name: string;
  choice: QuestionChoice[];
}

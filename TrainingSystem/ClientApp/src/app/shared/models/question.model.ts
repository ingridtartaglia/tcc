import { QuestionChoice } from './question-choice.model';

export class Question {
  constructor(examId: number) {
    this.examId = examId;
    this.questionChoices = [new QuestionChoice(), new QuestionChoice(), new QuestionChoice()];
  }
  examId: number;
  questionId: number;
  name: string;
  questionChoices: QuestionChoice[];
  choiceId: number;
}

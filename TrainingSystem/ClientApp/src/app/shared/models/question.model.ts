import { QuestionChoice } from './question-choice.model';

export class Question {
  questionId: number;
  questionName: string;
  questionChoices: QuestionChoice[];
}

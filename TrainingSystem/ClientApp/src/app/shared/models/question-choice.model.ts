export class QuestionChoice {
  constructor(questionId: number) {
    this.questionId = questionId;
  }
  questionId: number;
  questionChoiceId: number;
  name: string;
  isCorrect: boolean;
}

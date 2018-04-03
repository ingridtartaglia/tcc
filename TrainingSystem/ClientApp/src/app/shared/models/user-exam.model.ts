import { UserExamChoice } from './user-exam-choice.model';

export class UserExam {
  constructor(examId: number) {
    this.examId = examId;
    this.userExamChoices = [new UserExamChoice(), new UserExamChoice(), new UserExamChoice()];
  }
  userExamId: number;
  employeeId: number;
  examId: number;
  userExamChoices: UserExamChoice[];
  isApproved: boolean;
  submissionDate: Date;
}

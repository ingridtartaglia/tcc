import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';

import { Exam } from '../../shared/models/exam.model';
import { ExamService } from '../../shared/services/exam.service';
import { Question } from '../../shared/models/question.model';
import { QuestionService } from '../../shared/services/question.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-lesson-exam',
  templateUrl: './lesson-exam.component.html',
  styleUrls: ['./lesson-exam.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LessonExamComponent implements OnInit {
  @Input() exam: Exam;
  @Input() lessonId: number;
  @Output() updateLessonDetail = new EventEmitter<any>();
  newExam: Exam;
  questions: Question[];
  newQuestion: Question;
  isQuestionFormVisible: Boolean = false;
  showAnswerErrorMessage: Boolean = false;

  constructor(private examService: ExamService,
    private alertService: AlertService,
    private questionService: QuestionService) { }

  ngOnInit() {
    if (!this.exam) {
      this.newExam = new Exam(this.lessonId);
    } else {
      this.newQuestion = new Question(this.exam.examId);
    }
  }

  addExam() {
    this.examService.create(this.newExam)
      .subscribe(
        data => {
          this.updateLessonDetail.emit();
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  deleteExam(id: number) {
    this.examService.delete(id)
      .subscribe(
        data => {
          this.updateLessonDetail.emit();
          this.alertService.success('Prova deletada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  showQuestionForm() {
    this.isQuestionFormVisible = true;
  }

  addQuestion() {
    if (this.newQuestion.questionChoices.filter(choice => choice.isCorrect).length > 1) {
      this.showAnswerErrorMessage = true;
      return;
    }

    if (this.newQuestion.questionChoices.filter(choice => choice.isCorrect).length === 0) {
      this.showAnswerErrorMessage = true;
      return;
    }

    this.questionService.create(this.newQuestion)
      .subscribe(
        data => {
          this.newQuestion = new Question(this.exam.examId);
          this.isQuestionFormVisible = false;
          this.updateLessonDetail.emit();
          this.alertService.success('Questão adicionada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  deleteQuestion(id: number) {
    this.questionService.delete(id)
      .subscribe(
        data => {
          this.updateLessonDetail.emit();
          this.alertService.success('Questão deletada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

}

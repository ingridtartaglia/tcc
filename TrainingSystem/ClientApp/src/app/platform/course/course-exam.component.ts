import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Exam } from '../../shared/models/exam.model';
import { ExamService } from '../../shared/services/exam.service';
import { UserExam } from '../../shared/models/user-exam.model';
import { UserExamService } from '../../shared/services/user-exam.service';
import { UserExamChoice } from '../../shared/models/user-exam-choice.model';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-exam',
  templateUrl: './course-exam.component.html',
  styleUrls: ['./course-exam.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseExamComponent implements OnInit {
  exam: Exam;
  newUserExam: UserExam;
  selectedChoices: object;
  isExamSubmitted: boolean;
  isApproved: boolean;

  constructor(private examService: ExamService,
    private alertService: AlertService,
    private userExamService: UserExamService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getExam();
  }

  getExam() {
    const id = this.route.snapshot.params.examId;
    this.examService.getById(id).subscribe(exam => this.exam = exam);
  }

  submitExam() {
    this.newUserExam = new UserExam(this.exam.examId);
    this.exam.questions.forEach((question, index) => {
      this.newUserExam.userExamChoices.push(new UserExamChoice(question.choiceId));
    });

    this.userExamService.create(this.newUserExam)
      .subscribe(
        data => {
          this.isExamSubmitted = true;
          this.isApproved = data.isApproved;
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  reload() {
    window.location.reload();
  }
}

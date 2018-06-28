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
  loading = false;

  constructor(private examService: ExamService,
    private alertService: AlertService,
    private userExamService: UserExamService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getExam();
    this.getUserExam();
  }

  getUserExam() {
    const id = this.route.snapshot.params.examId;
    return this.userExamService.isApproved(id).subscribe(ue => this.isApproved = ue);
  }

  getExam() {
    const id = this.route.snapshot.params.examId;
    this.examService.getById(id).subscribe(exam => this.exam = exam);
  }

  submitExam() {
    this.loading = true;
    this.newUserExam = new UserExam(this.exam.examId);
    this.exam.questions.forEach((question, index) => {
      this.newUserExam.userExamChoices.push(new UserExamChoice(question.choiceId));
    });

    this.userExamService.create(this.newUserExam)
      .subscribe(
        data => {
          this.isExamSubmitted = true;
          this.isApproved = data.isApproved;
          this.loading = false;
        },
        error => {
          this.loading = false;
          this.alertService.error(error);
        }
      );
  }

}

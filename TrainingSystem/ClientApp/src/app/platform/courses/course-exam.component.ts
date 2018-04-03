import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Exam } from '../../shared/models/exam.model';
import { ExamService } from '../../shared/services/exam.service';
import { QuestionService } from '../../shared/services/question.service';
import { UserExam } from '../../shared/models/user-exam.model';
import { UserExamService } from '../../shared/services/user-exam.service';

@Component({
  selector: 'app-course-exam',
  templateUrl: './course-exam.component.html',
  styleUrls: ['./course-exam.component.css']
})
export class CourseExamComponent implements OnInit {
  exam: Exam;
  newUserExam: UserExam;
  selectedChoices: object;

  constructor(private examService: ExamService,
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
    this.exam.questions.forEach((question, index) => this.newUserExam.userExamChoices[index].questionChoiceId = question.choiceId);

    this.userExamService.create(this.newUserExam)
      .subscribe(
        data => {
          console.log(data);
        },
        error => {
          console.error();
        }
      );
  }
}

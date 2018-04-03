import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Question } from '../../shared/models/question.model';
import { Exam } from '../../shared/models/exam.model';
import { ExamService } from '../../shared/services/exam.service';
import { QuestionService } from '../../shared/services/question.service';

@Component({
  selector: 'app-course-exam',
  templateUrl: './course-exam.component.html',
  styleUrls: ['./course-exam.component.css']
})
export class CourseExamComponent implements OnInit {
  exam: Exam;
  questions: Question[];

  constructor(private examService: ExamService,
    private questionService: QuestionService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getExam();
  }

  getExam() {
    const id = this.route.snapshot.params.examId;
    this.examService.getById(id).subscribe(exam => this.exam = exam);
  }

  submitExam() {

  }
}

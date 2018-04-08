import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as jsPDF from 'jspdf';
import * as html2canvas from 'html2canvas';

import { Course } from '../../shared/models/course.model';
import { CourseService } from '../../shared/services/course.service';

@Component({
  selector: 'app-course-certificate',
  templateUrl: './course-certificate.component.html',
  styleUrls: ['./course-certificate.component.css']
})
export class CourseCertificateComponent implements OnInit {
  // @ViewChild('certificate') el: ElementRef;
  course: Course;

  constructor(private courseService: CourseService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getCourse();
  }

  getCourse() {
    const id = this.route.snapshot.params.courseId;
    this.courseService.getById(id).subscribe(course => this.course = course);
  }

  download(course: any) {
    html2canvas(document.getElementById('certificate')).then(function (canvas) {
      const doc = new jsPDF('l', 'mm', 'a4');
      const img = canvas.toDataURL('image/png');
      const width = doc.internal.pageSize.width; // 210 mm
      const height = doc.internal.pageSize.height; // 297 mm
      const courseName = course.name.replace(/ /g, '_');

      // (image, format, xPosition, yPosition, width, height)
      doc.addImage(img, 'JPEG', 10, 10, width, height);
      doc.save(`Certificado_${course.appUser.firstName}_${courseName}.pdf`);
    });
  }

}

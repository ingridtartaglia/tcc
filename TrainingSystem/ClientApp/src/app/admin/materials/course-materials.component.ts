import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { Material } from '../../shared/models/material.model';
import { MaterialService } from '../../shared/services/material.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-materials',
  templateUrl: './course-materials.component.html',
  styleUrls: ['./course-materials.component.scss']
})
export class CourseMaterialsComponent implements OnInit {
  @Input() materials: Material[];
  @Input() courseId: number;
  @Output() updateCourseDetail = new EventEmitter<any>();
  isMaterialFormVisible: Boolean = false;
  newMaterial: Material;
  fileSelected: Boolean = false;
  isFileTypeSupported: Boolean = false;

  constructor(private materialService: MaterialService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newMaterial = new Material(this.courseId);
  }

  showMaterialForm() {
    this.isMaterialFormVisible = true;
  }

  addMaterial() {
    this.materialService.create(this.newMaterial)
      .subscribe(
        data => {
          this.newMaterial = new Material(this.courseId);
          this.isMaterialFormVisible = false;
          this.updateCourseDetail.emit();
          this.alertService.success('Material adicionado com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  deleteMaterial(id: number) {
    this.materialService.delete(id)
      .subscribe(
        data => {
          this.updateCourseDetail.emit();
          this.alertService.success('Material deletado com sucesso!');
        },
        error => {
          this.alertService.error(error);
        }
      );
  }

  fileChange(files: FileList) {
    if (files && files[0].size > 0) {
      if (files[0].type === 'application/msword'
      || files[0].type === 'application/vnd.ms-powerpoint'
      || files[0].type === 'application/pdf') {
        this.isFileTypeSupported = true;
      } else {
        this.isFileTypeSupported = false;
      }

      this.newMaterial.file = files[0];
      this.fileSelected = true;
    }
  }

}

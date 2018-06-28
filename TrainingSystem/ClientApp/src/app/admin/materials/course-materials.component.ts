import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { Material } from '../../shared/models/material.model';
import { MaterialService } from '../../shared/services/material.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-materials',
  templateUrl: './course-materials.component.html',
  styleUrls: ['./course-materials.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseMaterialsComponent implements OnInit {
  @Input() materials: Material[];
  @Input() courseId: number;
  @Output() updateCourseDetail = new EventEmitter<any>();
  isMaterialListVisible: Boolean = true;
  isMaterialFormVisible: Boolean = false;
  newMaterial: Material;
  fileSelected: Boolean = false;
  isFileTypeSupported: Boolean = false;
  loading = false;

  constructor(private materialService: MaterialService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.newMaterial = new Material(this.courseId);
  }

  showMaterialForm() {
    this.isMaterialFormVisible = true;
    this.isMaterialListVisible = false;
  }

  backToMaterialList() {
    this.isMaterialListVisible = true;
    this.isMaterialFormVisible = false;
  }

  addMaterial() {
    this.loading = true;
    this.materialService.create(this.newMaterial)
      .subscribe(
        data => {
          this.newMaterial = new Material(this.courseId);
          this.updateCourseDetail.emit();
          this.fileSelected = false;
          this.backToMaterialList();
          this.alertService.success('Material adicionado com sucesso!');
          this.loading = false;
        },
        error => {
          this.loading = false;
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
      if ((files[0].type === 'application/msword'
        || files[0].type === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
        || files[0].type === 'application/vnd.ms-powerpoint'
        || files[0].type === 'application/vnd.openxmlformats-officedocument.presentationml.presentation'
        || files[0].type === 'application/pdf') && files[0].size < 31457280) {
        this.isFileTypeSupported = true;
      } else {
        this.isFileTypeSupported = false;
      }

      this.newMaterial.file = files[0];
      this.fileSelected = true;
    }
  }

}

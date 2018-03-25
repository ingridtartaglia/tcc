import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { Material } from '../../shared/models/material.model';
import { MaterialService } from '../../shared/services/material.service';

@Component({
  selector: 'app-course-materials',
  templateUrl: './course-materials.component.html',
  styleUrls: ['./course-materials.component.css']
})
export class CourseMaterialsComponent implements OnInit {
  @Input() materials: Material[];
  @Input() courseId: number;
  isMaterialFormVisible: Boolean = false;
  newMaterial: Material;
  fileSelected: Boolean = false;
  isFileTypeSupported: Boolean = false;

  constructor(private materialService: MaterialService, private router: Router) { }

  ngOnInit() {
    this.newMaterial = new Material();
    this.newMaterial.courseId = this.courseId;
  }

  showMaterialForm() {
    this.isMaterialFormVisible = true;
  }

  addMaterial() {
    this.materialService.create(this.newMaterial)
      .subscribe(
        data => {
          console.log('sucesso');
        },
        error => {
          console.error();
        }
      );
  }

  deleteMaterial(id: number) {
    this.materialService.delete(id)
      .subscribe(
        data => {
          console.log('sucesso');
        },
        error => {
          console.error();
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

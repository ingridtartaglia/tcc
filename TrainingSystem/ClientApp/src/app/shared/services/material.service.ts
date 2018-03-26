import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';

import { Material } from '../models/material.model';

@Injectable()
export class MaterialService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<Material[]> {
    return this.http.get<Material[]>('/api/Materials');
  }

  getById(materialId: number): Observable<Material> {
    return this.http.get<Material>(`/api/Materials/${materialId}`);
  }

  create(material: Material): Observable<any> {
    const formData = new FormData();
    formData.append('name', material.name);
    formData.append('file', material.file);
    formData.append('courseId', material.courseId.toString());

    return this.http.post('/api/Materials', formData);
  }

  delete(materialId: number): Observable<any> {
    return this.http.delete(`/api/Materials/${materialId}`);
  }

}

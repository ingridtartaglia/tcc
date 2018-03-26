export class Material {
  constructor(courseId: number) {
    this.courseId = courseId;
  }
  materialId: number;
  name: string;
  fileName: string;
  courseId: number;
  file: File;
}

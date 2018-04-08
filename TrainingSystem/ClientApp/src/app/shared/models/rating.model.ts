export class Rating {
  constructor(courseId: number) {
    this.courseId = courseId;
  }
  ratingId: number;
  courseId: number;
  grade: string;
  comment: string;
}

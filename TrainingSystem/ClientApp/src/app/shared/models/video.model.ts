export class Video {
  constructor(lessonId: number) {
    this.lessonId = lessonId;
  }
  videoId: number;
  name: string;
  fileName: string;
  lessonId: number;
  file: File;
}

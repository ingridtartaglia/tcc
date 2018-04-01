import { VideoWatch } from "./video-watch.model";

export class Video {
  constructor(lessonId: number) {
    this.lessonId = lessonId;
  }
  videoId: number;
  name: string;
  fileName: string;
  lessonId: number;
  file: File;
  videoWatch: VideoWatch;
}

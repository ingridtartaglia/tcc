import { Keyword } from './keyword.model';
import { Lesson } from './lesson.model';
import { Material } from './material.model';
import { Rating } from './rating.model';
import { VideoWatch } from './video-watch.model';
import { AppUser } from './app-user.model';

export class Course {
  courseId: number;
  name: string;
  category: string;
  instructor: string;
  description: string;
  isSubscribed: boolean;
  isCompleted: boolean;
  videosCount: number;
  courseRating: number;
  keywords: Keyword[];
  lessons: Lesson[];
  materials: Material[];
  ratings: Rating[];
  videoWatch: VideoWatch[];
  appUser: AppUser;
}

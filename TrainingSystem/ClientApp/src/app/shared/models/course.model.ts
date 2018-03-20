import { Keyword } from './keyword.model';
import { Lesson } from './lesson.model';
import { Material } from './material.model';
import { Rating } from './rating.model';

export class Course {
  courseId: number;
  name: string;
  category: string;
  instructor: string;
  description: string;
  keywords: Keyword[];
  lessons: Lesson[];
  materials: Material[];
  ratings: Rating[];
}

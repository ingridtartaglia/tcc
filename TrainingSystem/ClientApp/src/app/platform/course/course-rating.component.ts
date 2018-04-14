import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

import { Rating } from '../../shared/models/rating.model';
import { RatingService } from '../../shared/services/rating.service';
import { AlertService } from '../../shared/services/alert.service';

@Component({
  selector: 'app-course-rating',
  templateUrl: './course-rating.component.html',
  styleUrls: ['./course-rating.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CourseRatingComponent implements OnInit {
  @Input() isSubscribed: boolean;
  @Input() courseId: number;
  @Input() ratings: Rating[];
  newRating: Rating;

  constructor(private ratingService: RatingService, private alertService: AlertService) { }

  ngOnInit() {
    if (this.isSubscribed) {
      this.newRating = new Rating(this.courseId);
    }
  }

  addRating() {
    this.ratingService.create(this.newRating)
      .subscribe(
        data => {
          this.alertService.success('Sua nota para este curso foi adicionada com sucesso!');
        },
        error => {
          this.alertService.error(error);
        });
  }

}

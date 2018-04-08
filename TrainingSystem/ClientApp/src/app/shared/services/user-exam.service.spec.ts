import { TestBed, inject } from '@angular/core/testing';

import { UserExamService } from './user-exam.service';

describe('UserExamService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UserExamService]
    });
  });

  it('should be created', inject([UserExamService], (service: UserExamService) => {
    expect(service).toBeTruthy();
  }));
});

import { TestBed, inject } from '@angular/core/testing';

import { VideoWatchService } from './video-watch.service';

describe('VideoWatchService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VideoWatchService]
    });
  });

  it('should be created', inject([VideoWatchService], (service: VideoWatchService) => {
    expect(service).toBeTruthy();
  }));
});

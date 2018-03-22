import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonVideosComponent } from './lesson-videos.component';

describe('LessonVideosComponent', () => {
  let component: LessonVideosComponent;
  let fixture: ComponentFixture<LessonVideosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LessonVideosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonVideosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

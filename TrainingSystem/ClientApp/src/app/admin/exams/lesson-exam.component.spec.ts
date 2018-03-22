import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonExamComponent } from './lesson-exam.component';

describe('LessonExamComponent', () => {
  let component: LessonExamComponent;
  let fixture: ComponentFixture<LessonExamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LessonExamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LessonExamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

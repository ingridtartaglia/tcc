import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllCoursesListComponent } from './all-courses-list.component';

describe('AllCoursesListComponent', () => {
  let component: AllCoursesListComponent;
  let fixture: ComponentFixture<AllCoursesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllCoursesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllCoursesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

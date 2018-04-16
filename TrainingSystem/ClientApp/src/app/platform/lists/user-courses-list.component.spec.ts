import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCoursesListComponent } from './user-courses-list.component';

describe('UserCoursesListComponent', () => {
  let component: UserCoursesListComponent;
  let fixture: ComponentFixture<UserCoursesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserCoursesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserCoursesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

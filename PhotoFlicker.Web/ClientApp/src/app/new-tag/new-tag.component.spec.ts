import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewTagComponent } from './new-tag.component';

describe('NewTagComponent', () => {
  let component: NewTagComponent;
  let fixture: ComponentFixture<NewTagComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewTagComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

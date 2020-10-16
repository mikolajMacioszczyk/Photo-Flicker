import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoSliderComponent } from './photo-slider.component';

describe('PhotoSliderComponent', () => {
  let component: PhotoSliderComponent;
  let fixture: ComponentFixture<PhotoSliderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotoSliderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotoSliderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

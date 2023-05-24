import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedWithMePageComponent } from './shared-with-me-page.component';

describe('SharedWithMePageComponent', () => {
  let component: SharedWithMePageComponent;
  let fixture: ComponentFixture<SharedWithMePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SharedWithMePageComponent]
    });
    fixture = TestBed.createComponent(SharedWithMePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

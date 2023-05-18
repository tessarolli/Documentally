import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecoverPasswordPageComponent } from './recover-password-page.component';

describe('RecoverPasswordPageComponent', () => {
  let component: RecoverPasswordPageComponent;
  let fixture: ComponentFixture<RecoverPasswordPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RecoverPasswordPageComponent]
    });
    fixture = TestBed.createComponent(RecoverPasswordPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

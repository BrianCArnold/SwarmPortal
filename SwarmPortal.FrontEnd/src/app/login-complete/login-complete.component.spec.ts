import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginCompleteComponent } from './login-complete.component';

describe('LoginCompleteComponent', () => {
  let component: LoginCompleteComponent;
  let fixture: ComponentFixture<LoginCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginCompleteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

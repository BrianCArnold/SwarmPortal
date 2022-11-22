import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckboxRenderer } from './checkbox.component';

describe('CheckboxComponent', () => {
  let component: CheckboxRenderer;
  let fixture: ComponentFixture<CheckboxRenderer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckboxRenderer ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckboxRenderer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

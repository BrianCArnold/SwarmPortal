import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusScreenComponent } from './status-screen.component';

describe('StatusScreenComponent', () => {
  let component: StatusScreenComponent;
  let fixture: ComponentFixture<StatusScreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatusScreenComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatusScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

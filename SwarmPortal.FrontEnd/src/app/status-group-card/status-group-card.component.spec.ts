import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatusGroupCardComponent } from './status-group-card.component';

describe('StatusGroupCardComponent', () => {
  let component: StatusGroupCardComponent;
  let fixture: ComponentFixture<StatusGroupCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatusGroupCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatusGroupCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

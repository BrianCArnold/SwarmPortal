import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkGroupCardComponent } from './link-group-card.component';

describe('LinkGroupCardComponent', () => {
  let component: LinkGroupCardComponent;
  let fixture: ComponentFixture<LinkGroupCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LinkGroupCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LinkGroupCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

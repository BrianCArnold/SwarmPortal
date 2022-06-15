import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteRenderer } from './delete.component';

describe('DeleteComponent', () => {
  let component: DeleteRenderer;
  let fixture: ComponentFixture<DeleteRenderer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteRenderer ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteRenderer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

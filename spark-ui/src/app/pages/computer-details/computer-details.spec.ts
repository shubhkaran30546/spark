import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComputerDetails } from './computer-details';

describe('ComputerDetails', () => {
  let component: ComputerDetails;
  let fixture: ComponentFixture<ComputerDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComputerDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComputerDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

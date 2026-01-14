import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComputerList } from './computer-list';

describe('ComputerList', () => {
  let component: ComputerList;
  let fixture: ComponentFixture<ComputerList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComputerList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComputerList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

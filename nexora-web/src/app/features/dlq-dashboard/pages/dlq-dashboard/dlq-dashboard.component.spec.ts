import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DlqDashboardComponent } from './dlq-dashboard.component';

describe('DlqDashboardComponent', () => {
  let component: DlqDashboardComponent;
  let fixture: ComponentFixture<DlqDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DlqDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DlqDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

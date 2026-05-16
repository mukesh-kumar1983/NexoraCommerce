import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DlqDashboardRoutingModule } from './dlq-dashboard-routing.module';
import { DlqDashboardComponent } from './pages/dlq-dashboard/dlq-dashboard.component';


@NgModule({
  declarations: [
    DlqDashboardComponent
  ],
  imports: [
    CommonModule,
    DlqDashboardRoutingModule
  ]
})
export class DlqDashboardModule { }

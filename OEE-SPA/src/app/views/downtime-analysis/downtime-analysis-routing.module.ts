import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DowntimeAnalysisComponent } from './downtime-analysis.component';

const routes: Routes = [
  {
    path: '',
    component: DowntimeAnalysisComponent,
    data: {
      title: 'Downtime Analysis'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DowntimeAnalysisRoutingModule {}

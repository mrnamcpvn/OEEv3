import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DowntimeReasonsComponent } from './downtime-reasons.component';

const routes: Routes = [
  {
    path: '',
    component: DowntimeReasonsComponent,
    data: {
      title: 'Downtime Reasons'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DowntimeReasonsRoutingModule {}

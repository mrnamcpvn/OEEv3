import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';
import { LoginComponent } from './views/login/login.component';
import { AuthGuard } from './_guard/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'dashboard',
      //  canActivate: [AuthGuard],
        loadChildren: () => import('./views/dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'trend',
      //  canActivate: [AuthGuard],
        loadChildren: () => import('./views/trend/trend.module').then(m => m.TrendModule)
      },
      {
        path: 'downtime-reasons',
        canActivate: [AuthGuard],
        loadChildren: () => import('./views/downtime-reasons/downtime-reasons.module').then(m => m.DowntimeReasonsModule)
      },
      {
        path: 'downtime-analysis',
      //     canActivate: [AuthGuard],
        loadChildren: () => import('./views/downtime-analysis/downtime-analysis.module').then(m => m.DowntimeAnalysisModule)
      },
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}

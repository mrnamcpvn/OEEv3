import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { LoginComponent } from './login.component';
import {Injectable} from '@angular/core';
import { AuthGuard } from 'src/app/_guard/auth.guard';
import { TrendComponent } from '../trend/trend.component';
import { DowntimeReasonsComponent } from '../downtime-reasons/downtime-reasons.component';

const routes: Routes = [
  { path: 'dashboard',
component: DashboardComponent, canActivate: [AuthGuard]
}, // <---- connected Route with guard },
{ path: 'trend',
component: TrendComponent, canActivate: [AuthGuard]
}, // <---- connected Route with guard },
{ path: 'downtime-reasons',
component: DowntimeReasonsComponent, canActivate: [AuthGuard]
}, // <---- connected Route with guard },

{ path: 'login',
component: LoginComponent }
];

export const LoginRoutes = RouterModule.forChild(routes);

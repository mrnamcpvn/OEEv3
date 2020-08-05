import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { NgSelect2Module } from 'ng-select2';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import {A2Edatetimepicker} from 'ng2-eonasdan-datetimepicker';
import { SevenSegModule } from 'ng-sevenseg';
import { ChartModule, HIGHCHARTS_MODULES } from 'angular-highcharts';
import * as more from 'highcharts/highcharts-more.src';
import * as exporting from 'highcharts/modules/exporting.src';
import { NgxSpinnerModule } from 'ngx-spinner';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardChartComponent } from './dashboard-chart/dashboard-chart.component';
import { DashboardSearchComponent } from './dashboard-search/dashboard-search.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoutingModule,
    ChartsModule,
    BsDropdownModule,
    ButtonsModule.forRoot(),
    NgSelect2Module,
    A2Edatetimepicker,
    SevenSegModule,
    ChartModule,
    NgxSpinnerModule
  ],
  providers: [
    { provide: HIGHCHARTS_MODULES, useFactory: () => [ more, exporting ] } // add as factory to your providers
  ],
  declarations: [ DashboardComponent, DashboardChartComponent, DashboardSearchComponent ]
})
export class DashboardModule { }

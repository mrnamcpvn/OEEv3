import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgSelect2Module } from 'ng-select2';
import {A2Edatetimepicker} from 'ng2-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { DowntimeAnalysisComponent } from './downtime-analysis.component';
import { DowntimeAnalysisRoutingModule } from './downtime-analysis-routing.module';
import { AnalysisChartComponent } from './downtime-analysis-chart/analysis-chart.component';
import { PaginationComponent, PaginationModule } from 'ngx-bootstrap';

@NgModule({
  imports: [
    CommonModule,
    DowntimeAnalysisRoutingModule,
    ChartsModule,
    BsDropdownModule,
    NgSelect2Module,
    A2Edatetimepicker,
    FormsModule,
    PaginationModule
  ],
  declarations: [ DowntimeAnalysisComponent, AnalysisChartComponent ]
})
export class DowntimeAnalysisModule { }

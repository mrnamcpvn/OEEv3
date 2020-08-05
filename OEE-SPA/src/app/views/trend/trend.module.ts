import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartsModule } from 'ng2-charts';
import { NgSelect2Module } from 'ng-select2';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ChartModule } from 'angular-highcharts';
import { NgxSpinnerModule } from 'ngx-spinner';

import { TrendComponent } from './trend.component';
import { TrendRoutingModule } from './trend-routing.module';
import { TrendChartComponent } from './trend-chart/trend-chart.component';
import { TrendSearchComponent } from './trend-search/trend-search.component';


@NgModule({
  imports: [
    CommonModule,
    TrendRoutingModule,
    ChartsModule,
    BsDropdownModule.forRoot(),
    ButtonsModule.forRoot(),
    NgSelect2Module,
    FormsModule,
    ChartModule,
    NgxSpinnerModule
  ],
  declarations: [ TrendComponent, TrendChartComponent, TrendSearchComponent ]
})
export class TrendModule { }

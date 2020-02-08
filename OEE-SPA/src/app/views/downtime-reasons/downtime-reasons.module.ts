import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgSelect2Module } from 'ng-select2';
import {A2Edatetimepicker} from 'ng2-eonasdan-datetimepicker';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { DowntimeReasonsComponent } from './downtime-reasons.component';
import { DowntimeReasonsRoutingModule } from './downtime-reasons-routing.module';

@NgModule({
  imports: [
    CommonModule,
    DowntimeReasonsRoutingModule,
    ChartsModule,
    BsDropdownModule,
    NgSelect2Module,
    A2Edatetimepicker,
    FormsModule
  ],
  declarations: [DowntimeReasonsComponent]
})
export class DowntimeReasonsModule { }

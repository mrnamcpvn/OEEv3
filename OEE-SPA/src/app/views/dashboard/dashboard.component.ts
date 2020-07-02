import { Component, OnInit, OnDestroy } from '@angular/core';
import { AvailabilityService } from '../../_core/_services/availability.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, RouterModule } from '@angular/router';
export interface DataSearch {
  factory: string;
  building: string;
  machine_type: string;
  shift: string;
  fromTime: string;
  toTime: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: 'dashboard.component.html'
})

export class DashboardComponent implements OnInit, OnDestroy {
  data: DataSearch;
  dataChart: Array<{ name: string, value: number, colorOn: string, colorOff: string }>;
  autoload: any;
  // tslint:disable-next-line: max-line-length
  constructor(private availabilityService: AvailabilityService, private spinner: NgxSpinnerService, private router: Router) { }

  ngOnInit() {
    this.autoloadStart();
    // this.loadChart();
  }

  ngOnDestroy() {
    this.autoloadRemove();
  }
  // function put data to chart
  dataDashboard(value: DataSearch) {
    this.data = value;
    this.loadChart();
  }
  loadChart() {
    this.autoloadRemove();
    this.spinner.show();
    // tslint:disable-next-line: max-line-length
    this.availabilityService. getAvailability(this.data.factory, this.data.building, this.data.machine_type, this.data.shift, this.data.fromTime, this.data.toTime).subscribe(res => {
      this.dataChart = Object.entries(res).map(([key, value]) => {
        return {
          name: key,
          value: Number(value),
          colorOn: ((Number(value) >= 60 && Number(value) < 85) ? '#ffb822' : Number(value) >= 85 ? 'Lime' : 'Red'),
          colorOff: '#000'
        };
      });
      this.spinner.hide();
      this.autoloadStart();
    }, error => {
      this.spinner.hide();
      console.log(error);
    });
  }

  // Auto load chart after 30s
  autoloadStart() {
    this.autoload = setInterval(() => {
      this.loadChart();
    }, 600000);
  }

  // clear auto load chart
  autoloadRemove() {
    if (this.autoload) {
      clearInterval(this.autoload);
    }
  }
}

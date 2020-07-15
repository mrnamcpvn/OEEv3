import { Component, OnInit, OnDestroy } from '@angular/core';
import { TrendService } from '../../_core/_services/trend.service';
import { NgxSpinnerService } from 'ngx-spinner';

export interface DataSearch {
  factory: string;
  building: string;
  machine_type: string;
  shift: string;
  typeTime: string;
  numberTime: string;
  count: number;
}

@Component({
  selector: 'app-trend',
  templateUrl: './trend.component.html'
})
export class TrendComponent implements OnInit, OnDestroy {
  // chart
  dataChart: Array<{ label: string, data: Array<number> }>;
  labels: Array<string>;
  autoload: any;
  isShowTable: boolean = false;
  arrayNull: Array<string> = [];
  data: DataSearch;

  constructor(private trendService: TrendService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.autoloadStart();
  }

  ngOnDestroy() {
    this.autoloadRemove();
  }

  dataTrend(value: DataSearch) {
    this.data = value;
    this.loadChart();
  }

  loadChart() {
    this.autoloadRemove();
    this.spinner.show();
    this.isShowTable = false;
    this.trendService.getAvailability(this.data)
      .subscribe(res => {
        if(res != null) {
          console.log(res);
          //   this.arrayNull.pop();
          //   this.arrayNull.push('demo');
          //   this.dataChart = res.dataChart.map(item => {
          //     return { label: item.name, data: item.data };
          //   });
          //   this.labels = res.listTime;
          this.spinner.hide();
          //   this.isShowTable = true;
          //   this.autoloadStart();
          // }, error => {
          //   this.isShowTable = true;
          //   console.log(error);
        }
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

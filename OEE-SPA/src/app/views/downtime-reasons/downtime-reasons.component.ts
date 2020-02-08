import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeReasonsService } from '../../../app/_core/_services/downtime-reasons.service';
import { ActionTime } from '../../../app/_core/_models/action-time';
declare var google: any;
@Component({
  selector: 'app-downtime-reasons',
  templateUrl: './downtime-reasons.component.html'
})
export class DowntimeReasonsComponent implements OnInit, AfterViewInit {
  @ViewChild('trendChart', { static: true }) trendChart: ElementRef;

  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  shift: string = '0';
  date: Date = new Date();
  dataActionTime: Array<ActionTime> = [];

  factories: Array<Select2OptionData> = [
    {
      id: 'ALL',
      text: 'All Factories'
    },
    {
      id: 'SHW',
      text: 'SHW'
    },
    {
      id: 'SHD',
      text: 'SHD'
    },
    {
      id: 'SHB',
      text: 'SHB'
    },
    {
      id: 'SY2',
      text: 'SY2'
    }
  ];

  buildings: Array<Select2OptionData>;

  public shifts: Array<Select2OptionData> = [
    {
      id: '0',
      text: 'All Shifts'
    },
    {
      id: '1',
      text: 'Shift 1'
    },
    {
      id: '2',
      text: 'Shift 2'
    }
  ];

  public machines: Array<Select2OptionData>;

  public optionsSelect2: Options = {
    theme: 'bootstrap4',
  };

  optionDatetimes: any = {
    date: this.date,
    format: 'YYYY-MM-DD',
    icons: {
      time: 'fa fa-clock-o',
      date: 'fa fa-calendar',
      up: 'fa fa-chevron-up',
      down: 'fa fa-chevron-down',
      previous: 'fa fa-chevron-left',
      next: 'fa fa-chevron-right',
      today: 'fa fa-calendar-check-o text-success',
      clear: 'fa fa-trash text-danger',
      close: 'fa fa-window-close-o text-secondary'
    },
    dayViewHeaderFormat: 'DD-MM-YYYY',
    sideBySide: true
  };

  constructor(private commonService: CommonService, private downtimeReasonsService: DowntimeReasonsService) { }

  ngOnInit() {
    this.loadChart();
  }

  changeFactory(value: any) {
    this.building = 'ALL';
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      // tslint:disable-next-line: triple-equals
      if (value == 'SHW' || value == 'SHD') {
        this.loadBuilding();
      } else {
        // vì database : SHB không có bảng ActionTime, SY2: không có building

        // tslint:disable-next-line: triple-equals
        if (value == 'SY2') {
          this.loadMachine();
        } else {
          this.buildings = [];
        }
      }
    } else {
      this.loadChart();
    }
  }

  changeBuilding(value: any) {
    this.building = value;
    this.machine = 'ALL';
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      this.loadMachine();
    } else {
      this.machines = null;
    }
  }

  changeMachine(value: any): any {
    this.machine = value;

    this.loadChart();
  }

  changeShift(event: any) {
    this.loadChart();
  }

  updateDate(event: any) {
    // tslint:disable-next-line: triple-equals
    if (formatDate(this.date, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.date = new Date(event.srcElement.value);
      this.loadChart();
    }
  }

  loadBuilding() {
    this.commonService.getBuildingsActionTime(this.factory).subscribe(res => {
      this.buildings = res.map((item, index, array) => {
        return {
          id: array[array.length - 1 - index] == null ? 'other' : array[array.length - 1 - index],
          text: array[array.length - 1 - index] == null ? 'Other' : ('Building ' + array[array.length - 1 - index])
        };
      });
      this.buildings.unshift({ id: 'ALL', text: 'All Building' });

    }, error => {
      console.log(error);
    });
  }

  loadMachine() {
    this.commonService.getMachinesActionTime(this.factory, this.building).subscribe(res => {
      this.machines = res.map(item => {
        return { id: item, text: item };
      });

      this.machines.unshift({ id: 'ALL', text: 'All Machine' });
    }, error => {
      console.log(error);
    });
  }

  loadChart() {
    // tslint:disable-next-line: max-line-length
    // console.log('f: ' + this.factory + ' m: ' + this.machine + ' s: ' + this.shift + ' d: ' + formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'));
    // this.downtimeReasonsService.getDowntimeReasons(this.factory, this.machine, this.shift, formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'))
    //   .subscribe(res => {
    //     this.dataActionTime = res;
    //     console.log(res);
    //   }, (error: any) => {
    //     console.log(error);
    //   });
  }

  ngAfterViewInit() {
    google.charts.load('current', { packages: ['timeline'] });
    google.charts.setOnLoadCallback(this.drawChart);
  }

  onResize(event: any) {
    google.charts.load('current', { packages: ['timeline'] });
    google.charts.setOnLoadCallback(this.drawChart);
  }

  drawChart = () => {

    const data = google.visualization.arrayToDataTable([
      ['RUN', new Date(0, 0, 0, 7, 0, 0), new Date(0, 0, 0, 7, 20, 0)],
      ['RUN', new Date(0, 0, 0, 7, 25, 0), new Date(0, 0, 0, 7, 40, 0)],
      ['RUN', new Date(0, 0, 0, 7, 45, 0), new Date(0, 0, 0, 8, 0, 0)],
      ['RUN', new Date(0, 0, 0, 9, 0, 0), new Date(0, 0, 0, 9, 30, 0)],
      ['RUN', new Date(0, 0, 0, 13, 0, 0), new Date(0, 0, 0, 14, 30, 0)],
      ['RUN', new Date(0, 0, 0, 15, 0, 0), new Date(0, 0, 0, 16, 0, 0)],

      ['IDLE', new Date(0, 0, 0, 7, 0, 0), new Date(0, 0, 0, 9, 20, 0)],
      ['IDLE', new Date(0, 0, 0, 10, 0, 0), new Date(0, 0, 0, 12, 10, 0)],
      ['IDLE', new Date(0, 0, 0, 13, 0, 0), new Date(0, 0, 0, 13, 5, 0)],
      ['IDLE', new Date(0, 0, 0, 13, 30, 0), new Date(0, 0, 0, 14, 0, 0)],
      ['IDLE', new Date(0, 0, 0, 14, 30, 0), new Date(0, 0, 0, 16, 0, 0)],

      ['IN', new Date(0, 0, 0, 8, 0, 0), new Date(0, 0, 0, 8, 20, 0)],
      ['IN', new Date(0, 0, 0, 9, 0, 0), new Date(0, 0, 0, 10, 8, 0)],
      ['IN', new Date(0, 0, 0, 11, 0, 0), new Date(0, 0, 0, 12, 0, 0)],
      ['IN', new Date(0, 0, 0, 12, 26, 0), new Date(0, 0, 0, 13, 0, 0)],
      ['IN', new Date(0, 0, 0, 13, 5, 0), new Date(0, 0, 0, 13, 30, 0)],
      ['IN', new Date(0, 0, 0, 14, 10, 0), new Date(0, 0, 0, 16, 0, 0)],
    ]);

    const options = {
      colors: ['#00a71c', '#e88b00', '#0354b9'],
      timeline: {
        rowLabelStyle: { fontName: 'Helvetica', fontSize: 18, color: '#0000' },
        barLabelStyle: { fontName: 'Garamond', fontSize: 12 }
      },
      tooltip: {
        isHtml: true
      }
    };

    const chart = new google.visualization.Timeline(this.trendChart.nativeElement);
    chart.draw(data, options);
  }

}

import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeAnalysisService } from '../../../app/_core/_services/downtime-analysis.service';
// import { base64 } from 'src/assets/libary/exceljs/dist/exceljs';
import { Pagination } from '../../_core/_models/pagination';
import { ChartReason } from '../../_core/_models/chart-reason';
import { Week } from 'src/app/_core/_models/week';

@Component({
  selector: 'app-downtime-analysis',
  templateUrl: './downtime-analysis.component.html'
})
export class DowntimeAnalysisComponent implements OnInit, AfterViewInit {
  typeTime = 'date';
  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  machine_type: string = 'ALL';
  shift: string = '0';
  week: string = '1';
  month: string = (new Date().getMonth() + 1).toString();

  date: Date = new Date();
  dateTo: Date = new Date();
  arrayWeek: Week[];

  dataActionTime: Array<ChartReason> = [];
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  dataChart: Array<{ title: string, data: Array<number>, labels: Array<string> }>;
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
  machine_types: Array<Select2OptionData>;

  times: Array<Select2OptionData> = [
    {
      id: 'date',
      text: 'Date'
    },
    {
      id: 'week',
      text: 'Week'
    },
    {
      id: 'month',
      text: 'Month'
    }
  ];

  months: Array<Select2OptionData> = [
    { id: '1', text: 'January' },
    { id: '2', text: 'February' },
    { id: '3', text: 'March' },
    { id: '4', text: 'April' },
    { id: '5', text: 'May' },
    { id: '6', text: 'June' },
    { id: '7', text: 'July' },
    { id: '8', text: 'August' },
    { id: '9', text: 'September' },
    { id: '10', text: 'October' },
    { id: '11', text: 'November' },
    { id: '12', text: 'December' },
  ];

  weeks: Array<Select2OptionData>;

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

  constructor(private commonService: CommonService,
    private downtimeAnalysisService: DowntimeAnalysisService) { }

  ngOnInit() {
    // this.dataChart = [
    //   {
    //     title: 'Down Time Reason Analysis',
    //     data: [51, 40, 30, 20, 10],
    //     labels: ['Manpower', 'Method', 'Material', 'Machine', 'Others']
    //   },
    //   {
    //     title: 'Down Time Reason Analysis',
    //     data: [101, 90, 80, 70, 60, 50, 40, 30, 20, 10],
    // tslint:disable-next-line: max-line-length
    //     labels: ['Unplaned Maintenance', 'Quality Issues', 'Production Trial', 'Lack of Material', 'Lack of Manpower', 'Development Trial', 'Change Punching', 'Change Knife', 'Adjust Material', 'Adjust Machine']
    //   }
    // ];
    this.loadWeeks();
    this.loadChart();
  }
  ngAfterViewInit() {
  }
  changeFactory(value: any) {
    // this.building = 'ALL';
    // if (value === 'SHB' || value === 'SY2') {
    //   this.building = value;
    // }
    // // tslint:disable-next-line: triple-equals
    // if (value != 'ALL') {
    //   this.loadChart();
    // } else {
    //   this.buildings = null;
    //   this.machines = null;
    // }
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
    this.loadMachine_Type();
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      this.loadMachine();
    } else {
      this.machines = null;
    }
    this.loadChart();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadChart();
  }
  changeMachine(value: any) {
    this.loadChart();
  }
  changeMachine_Type(event: any) {
    this.loadMachine();
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
 loadChart() {
  console.log('f: ' + this.factory + ' m: ' + this.machine + ' s: ' + this.shift + ' d: ' + formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'));
  // tslint:disable-next-line: max-line-length
  this.downtimeAnalysisService.getDowntimeAnalysis(this.factory,
                                                  this.building,
                                                  this.machine_type,
                                                  this.machine,
                                                   this.shift,
                                                  formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'),
                                                  formatDate(new Date(this.dateTo), 'yyyy-MM-dd', 'en-US'))
  .subscribe(res => {
    //  this.dataActionTime = res.result;
     // this.pagination = res.pagination;
     // this.drawChart(res.resultC);
     const arrayA = res.resB.map(function(item) {
      return [item['reason_1'].toString()];
     });
     const labelsA = res.resB.map(function(item) {
      return (item['duration']);
     });
     const arrayB = res.resA.map(function(item) {
      return [item['reason_2'].toString()];
     });
     const labelsB = res.resA.map(function(item) {
      return item['duration'];
     });

     this.dataChart = [
       {
        title: 'Level 1',
       data: labelsA,
       labels: arrayA
       },
       {
         title: 'Level 2',
         data: labelsB,
         labels: arrayB
       }
     ];
      console.log(res);
    }, (error: any) => {
      console.log(error);
    });
}
  loadBuilding() {
    this.commonService.getBuilding(this.factory).subscribe(res => {

      this.buildings = res.map(item => {
        return { id: item, text: 'Building ' + item };
      });
      this.buildings.unshift({ id: 'ALL', text: 'All Building' });

    }, error => {
      console.log(error);
    });
  }
// Machine_Types
loadMachine_Type() {
  this.commonService.getMachine_Type(this.factory, this.building).subscribe(res => {
    this.machine_types = res.map(item => {
      return { id: item, text:  item };
    });
    this.machine_types.unshift({ id: 'ALL', text: 'All Machine Types' });
  }, error => {
    console.log(error);
  });
}
  loadMachine() {
    this.commonService.getMachine(this.factory, this.building, this.machine_type).subscribe(res => {
      this.machines = res.map(item => {
        return { id: item, text: 'Machine ' + item };
      });
      this.machines.unshift({ id: 'ALL', text: 'All Machine' });

    }, error => {
      console.log(error);
    });
  }
  changeTypeTime(value: any) {
    if (value === 'date') {
      this.resetDate();
      this.loadChart();
    }
    if (value === 'week') {
      this.resetDate();
      this.changeWeek(this.week);
    }
    if (value === 'month') {
      this.resetDate();
      this.changeMonth(this.month);
    }
  }
  loadWeeks() {
    this.commonService.getWeeks().subscribe(res => {
      this.arrayWeek = res.map(item => {
        return {
          weekNum: item.weekNum,
          weekStart: item.weekStart,
          weekFinish: item.weekFinish
        };
      });

      this.weeks = res.map(item => {
        return {
          id: item.weekNum,
          text: 'Week ' + item.weekNum + ' (' + formatDate(item.weekStart, 'MM/dd', 'en-US') + ' - ' + formatDate(item.weekFinish, 'MM/dd', 'en-US') + ')'
        };
      });

      this.week = this.currentWeek();
    }, error => {
      console.log(error);
    });
  }
  resetDate() {
    this.date = new Date();
    this.month = (new Date().getMonth() + 1).toString();
    this.week = this.currentWeek();
  }
  changeWeek(event: any) {
    const time = this.arrayWeek[event - 1];
    this.date = new Date(time.weekStart);
    this.dateTo = new Date(time.weekFinish);
    this.loadChart();
  }
  changeMonth(event: any) {
    const time = this.commonService.getFistLastDayOfMonth(event);
    this.date = time.firstDate;
    this.dateTo = time.lastDate;
    this.loadChart();
  }

  currentWeek() {
    const currentWeek = this.arrayWeek.filter((item) => {
      return (
        new Date(new Date().toISOString().split('T')[0]) >= new Date(item.weekStart.split('T')[0]) &&
        new Date(new Date().toISOString().split('T')[0]) <= new Date(item.weekFinish.split('T')[0])
      );
    });
    return currentWeek.length === 0 ? '1' : currentWeek[0].weekNum;
  }

}

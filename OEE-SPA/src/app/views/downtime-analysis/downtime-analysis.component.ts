import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeAnalysisService } from '../../../app/_core/_services/downtime-analysis.service';
import { Pagination } from '../../_core/_models/pagination';
import { ChartReason } from '../../_core/_models/chart-reason';
import { Week } from '../../_core/_models/week';
import { de } from 'date-fns/locale';

@Component({
  selector: 'app-downtime-analysis',
  templateUrl: './downtime-analysis.component.html'
})
export class DowntimeAnalysisComponent implements OnInit, AfterViewInit {
  typeTime = 'date';
  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  machine_type: string = '0';
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
  factories: Array<Select2OptionData> = [];

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

  public shifts: Array<Select2OptionData> = [];

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
    console.log(this.uniqueInOrder('AAAABBBCCDAABBB'));
    this.loadFactory();
    this.loadShift();
    this.loadWeeks();
    this.loadChart();

  }
  ngAfterViewInit() {
  }
  changeFactory(value: any) {
    this.loadBuilding();
    this.loadChart();
  }

  changeBuilding(value: any) {
    this.loadMachine_Type();
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
    if (formatDate(this.date, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.date = new Date(event.srcElement.value);
      this.dateTo = new Date(event.srcElement.value);
      this.loadChart();
    }
  }
  loadChart() {
    this.downtimeAnalysisService.getDowntimeAnalysis(this.factory,
      this.building,
      this.machine_type,
      this.machine,
      this.shift,
      formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'),
      formatDate(new Date(this.dateTo), 'yyyy-MM-dd', 'en-US'))
      .subscribe(res => {
        const arrayA = res.resB.map(function (item) {
          return [item['reason_1'].toString()];
        });
        const labelsA = res.resB.map(function (item) {
          return (item['duration']);
        });
        const arrayB = res.resA.map(function (item) {
          return [item['reason_2'].toString()];
        });
        const labelsB = res.resA.map(function (item) {
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
      }, (error: any) => {
        console.log(error);
      });
  }
  loadShift() {
    this.commonService.getListShift().subscribe(res => {
      this.shifts = res.map(item => {
        return { id: item.shift_id.toString(), text: item.shift_name }
      });
      this.shifts.unshift({ id: '0', text: 'All Shifts' });
    });
  }
  loadFactory() {
    this.commonService.getListFactory().subscribe(res => {
      this.factories = res.map(item => {
        return { id: item.factory_id, text: item.customer_name };
      });
      this.factories.unshift({ id: 'ALL', text: 'ALL Factories' });
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
  loadMachine_Type() {
    this.commonService.getMachine_Type(this.factory, this.building).subscribe(res => {
      this.machine_types = res.map(item => {
        return { id: item.id, text: item.machine_type_name };
      });
      this.machine_types.unshift({ id: '0', text: 'All Machine Types' });
    }, error => {
      console.log(error);
    });
  }
  loadMachine() {
    this.commonService.getMachinesActionTime(this.factory, this.building, this.machine_type).subscribe(res => {
      this.machines = res.map(item => {
        return { id: item.machine_id, text: 'Machine ' + item.machine_id };
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
  uniqueInOrder(iterable) {
    // iterable is array
    if(Array.isArray(iterable)) {
      let arrSet = Array.from(new Set(iterable));
      return arrSet;
    }
    // iterable is not array
    else {
      let arrSet = iterable.toString().split('');
      arrSet = Array.from(new Set(arrSet));
      let stringConvert = arrSet.join('');
      return arrSet;
    }
  }
}
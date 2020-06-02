import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { formatDate } from '@angular/common';
import { Options } from 'select2';
import { CommonService } from '../../../_core/_services/common.service';
import { TrendService } from '../../../_core/_services/trend.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Select2OptionData } from 'ng-select2';
import { Week } from '../../../_core/_models/week';

@Component({
  selector: 'app-trend-search',
  templateUrl: './trend-search.component.html'
})
export class TrendSearchComponent implements OnInit {
  @Input() dataChart: Array<{ label: string, data: Array<number> }>;
  @Input() labels: Array<string>;
  @Output() dataTrend = new EventEmitter<{ factory: string, building: string, shift: string, typeTime: string, numberTime: string }>();

  typeTime = 'week';
  numberTime: string = '1';
  factory: string = 'ALL';
  machine_type: string = 'ALL';
  building: string = 'ALL';
  shift: string = '0';
  week: string = '1';
  month: string = (new Date().getMonth() + 1).toString();

  dataExport: Array<Array<string>> = [];
  arrayWeek: Week[];

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

  shifts: Array<Select2OptionData> = [
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

  times: Array<Select2OptionData> = [
    {
      id: 'week',
      text: 'Week'
    },
    {
      id: 'month',
      text: 'Month'
    },
    {
      id: 'year',
      text: 'Year'
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

  quaters: Array<Select2OptionData> = [
    { id: '1', text: 'Quarter 1' },
    { id: '2', text: 'Quarter 2' },
    { id: '3', text: 'Quarter 3' },
    { id: '4', text: 'Quarter 4' },
  ];

  weeks: Array<Select2OptionData>;

  optionsSelect2: Options = {
    theme: 'bootstrap4',
  };

  constructor(private commonService: CommonService, private trendService: TrendService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.loadWeeks();
  }

  exportExcel() {
    this.dataExport = [];
    this.dataChart.forEach(item => {
      let array = JSON.parse(JSON.stringify(item.data));
      array = array.map(function (x: string) { return (x + '%'); });

      // tslint:disable-next-line: triple-equals
      if (this.factory != 'ALL' && this.building == 'ALL') {
        // tslint:disable-next-line: triple-equals
        array.unshift(this.factory, item.label, this.shift == '0' ? 'ALL' : this.shift);
        // tslint:disable-next-line: triple-equals
      } else if (this.factory != 'ALL' && this.building != 'ALL') {
        // tslint:disable-next-line: triple-equals
        array.unshift(this.factory, this.building, item.label, this.shift == '0' ? 'ALL' : this.shift);
      } else {
        // tslint:disable-next-line: triple-equals
        array.unshift(item.label, this.shift == '0' ? 'ALL' : this.shift);
      }
      this.dataExport.push(array);
    });

    let label = JSON.parse(JSON.stringify(this.labels));
    label = label.map(function (x: string) { return ('Time: ' + x); });
    // tslint:disable-next-line: triple-equals
    if (this.factory != 'ALL' && this.building == 'ALL') {
      label.unshift('Factory', 'Building', 'Shift');
      // tslint:disable-next-line: triple-equals
    } else if (this.factory != 'ALL' && this.building != 'ALL') {
      label.unshift('Factory', 'Building', 'Machine', 'Shift');
    } else {
      label.unshift('Factory', 'Shift');
    }

    this.trendService.exportExcel(this.dataExport, label, this.factory, this.building);
  }

  changeFactory(value: any) {
    this.building = 'ALL';
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      this.loadBuilding();
    } else {
      this.loadChart();
    }
  }

  changeBuilding(event: any) {
    this.loadChart();
  }
  changeMachine_Type(event: any){
    this.loadChart();
  }
  changeShift(event: any) {
    this.loadChart();
  }

  changeWeek(event: any) {
    this.numberTime = event;
    this.loadChart();
  }

  changeMonth(event: any) {
    this.numberTime = event;
    this.loadChart();
  }

  changeTypeTime(value: any) {
    if (value === 'week') {
      this.resetTime();
      this.changeWeek(this.week);
    }
    if (value === 'month') {
      this.resetTime();
      this.changeMonth(this.month);
    }
    if (value === 'year') {
      this.loadChart();
    }
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
    // this.commonService.getMachine_Type(this.factory).subscribe(res => {
    //   this.machine_types = res.map(item => {
    //     return { id: item, text:  item };
    //   });
    //   this.machine_types.unshift({ id: 'ALL', text: 'All MACHINE TYPES' });
    // }, error => {
    //   console.log(error);
    // });
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

  resetTime() {
    this.month = (new Date().getMonth() + 1).toString();
    this.week = this.currentWeek();
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

  loadChart() {
    const data = {
      factory: this.factory,
      building: this.building,
      shift: this.shift,
      typeTime: this.typeTime,
      numberTime: this.numberTime
    };

    this.dataTrend.emit(data);
  }
}

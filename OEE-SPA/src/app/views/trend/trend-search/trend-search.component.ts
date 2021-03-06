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
  @Output() dataTrend = new EventEmitter<{ factory: string, building: string, shift: string, typeTime: string, numberTime: string}>();

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

  factories: Array<Select2OptionData> = [];

  buildings: Array<Select2OptionData>;
  machine_types: Array<Select2OptionData>;

  shifts: Array<Select2OptionData> = [];

  times: Array<Select2OptionData> = [
    {
      id: 'week',
      text: 'Week'
    },
    {
      id: 'month',
      text: 'Month'
    },
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
  count = 0;
  constructor(private commonService: CommonService, 
              private trendService: TrendService, ) { }

  ngOnInit() {
    this.getListFactory();
    this.getListShift();
    this.loadWeeks();
  }

  exportExcel() {
    this.dataExport = [];
    this.dataChart.forEach(item => {
      let array = JSON.parse(JSON.stringify(item.data));
      array = array.map(function (x: string) { return (x + '%'); });
      if (this.factory != 'ALL' && this.building == 'ALL') {
        array.unshift(this.factory, item.label, this.shift == '0' ? 'ALL' : this.shift);
      } else if (this.factory != 'ALL' && this.building != 'ALL') {
        array.unshift(this.factory, this.building, item.label, this.shift == '0' ? 'ALL' : this.shift);
      } else {
        array.unshift(item.label, this.shift == '0' ? 'ALL' : this.shift);
      }
      this.dataExport.push(array);
    });

    let label = JSON.parse(JSON.stringify(this.labels));
    label = label.map(function (x: string) { return ('Time: ' + x); });
    if (this.factory != 'ALL' && this.building == 'ALL') {
      label.unshift('Factory', 'Building', 'Shift');
    } else if (this.factory != 'ALL' && this.building != 'ALL') {
      label.unshift('Factory', 'Building', 'Machine', 'Shift');
    } else {
      label.unshift('Factory', 'Shift');
    }

    this.trendService.exportExcel(this.dataExport, label, this.factory, this.building);
  }
  getListFactory() {
    this.commonService.getListFactory().subscribe(res => {
      this.factories = res.map(item => {
        return { id: item.factory_id, text: item.customer_name}
      });
      this.factories.unshift({id: 'ALL',text: 'All Factories'});
    });
  }
  getListShift() {
    this.commonService.getListShift().subscribe(res => {
      this.shifts = res.map(item => {
        return {id: item.shift_id.toString(),text: item.shift_name}
      });
      this.shifts.unshift({id: '0',text: 'All Shifts'});
    });
  }
  changeFactory(value: any) {
    this.building = 'ALL';
    if (value != 'ALL') {
      this.loadBuilding();
    } else {
      this.loadChart();
    }
  }

  changeBuilding(value: any) {
    this.building = value;
    this.machine_type = "ALL";
    if(value !== "ALL") {
      this.loadMachine_Type();
    } else {
      this.loadChart();
    }
}
  changeMachine_Type(event: any) {
    this.machine_type = event.toString();
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
    if (value === 'date') {
      this.resetTime();
      this.loadChart();
    }
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
    this.commonService.getMachine_Type(this.factory, this.building).subscribe(res => {
      this.machine_types = res.map(item => {
        return { id: item.id, text:  item.machine_type_name };
      });
      this.machine_types.unshift({ id: 'ALL', text: 'All Machine Types' });
    }, error => {
      console.log(error);
    });
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
    }, error => {
      console.log(error);
    });
    setTimeout(() => {
      this.resetTime();
      this.numberTime = this.currentWeek();
      this.loadChart();
    }, 1000);

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
    this.count = this.count + 1;
    const data = {
      factory: this.factory,
      building: this.building,
      machine_type: this.machine_type,
      shift: this.shift,
      typeTime: this.typeTime,
      numberTime: this.numberTime,
    };
    if(this.count >=4) {
      this.dataTrend.emit(data);
    }
  }
}

import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Week } from '../../../_core/_models/week';
// import { Chart_availability } from 'src/app/_core/_models/chart_availability';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../../_core/_services/common.service';
import { AvailabilityService } from '../../../_core/_services/availability.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { formatDate } from '@angular/common';
import { FunctionUtility } from '../../../_core/_utility/function-utility';


@Component({
  selector: 'app-dashboard-search',
  templateUrl: './dashboard-search.component.html'
})
export class DashboardSearchComponent implements OnInit {
  @Input()  dataChart: Array<{ name: string, value: number, colorOn: string, colorOff: string }>;
  @Output() dataDashboard = new EventEmitter<{ factory: string, building: string, shift: string, month: string, date: string, dateTo: string}>();

  typeTime = 'date';
  factory: string = 'ALL';
  building: string = 'ALL';
  machine_type: string = 'ALL';
  shift: string = '0';
  week: string = '1';
  month: string = (new Date().getMonth() + 1).toString();
  quater: string = (Math.floor((new Date().getMonth() + 3) / 3)).toString();
  date: Date = new Date();
  dateTo: Date = new Date();
  arrayWeek: Week[];
  // chart: Chart_availability;
  autoload: any;
  dataExport: Array<Array<string>> = [];

  factories: Array<Select2OptionData> = [];

  buildings: Array<Select2OptionData>;
  machine_types: Array<Select2OptionData>;
  shifts: Array<Select2OptionData> = [];

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
    },
    {
      id: 'quarter',
      text: 'Quarter'
    },
    {
      id: 'range',
      text: 'Range Time'
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
    sideBySide: true,
    // toolbarPlacement: 'top',
    // showTodayButton: true,
    // showClear: true,
    // showClose: true,
  };
  constructor(private commonService: CommonService,
    private availabilityService: AvailabilityService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.getListFactory();
    this.getListShift();
    this.loadWeeks();
    //this.loadChart();
  }

  exportExcel() {
    this.dataExport = [];

    this.dataChart.forEach(item => {
      let timeExport: string;
      if (this.typeTime === 'date') {
        timeExport = formatDate(this.date, 'yyyy-MM-dd', 'en-US');
      }
      if (this.typeTime === 'week') {
        const tmp = this.weeks.filter((week) => {
          return week.id === this.week;
        });
        timeExport = tmp[0].text;
      }
      if (this.typeTime === 'month') {
        const tmp = this.months.filter((month) => {
          return month.id === this.month;
        });
        timeExport = tmp[0].text;
      }
      if (this.typeTime === 'quarter') {
        timeExport = 'Quarter ' + this.quater;
      }
      if (this.typeTime === 'ranger') {
        timeExport = 'Range (' + formatDate(this.date, 'yyyy-MM-dd', 'en-US') + formatDate(this.dateTo, 'yyyy-MM-dd', 'en-US') + ')';
      }
      const array: Array<string> = [JSON.parse(JSON.stringify(item.value + '%'))];
      if (this.factory != 'ALL' && this.building == 'ALL') {
        array.unshift(this.factory, item.name, this.shift == '0' ? 'ALL' : this.shift, timeExport);
      } else if (this.factory != 'ALL' && this.building != 'ALL') {
        array.unshift(this.factory, this.building, item.name, this.shift == '0' ? 'ALL' : this.shift, timeExport);
      } else {
        array.unshift(item.name, this.shift == '0' ? 'ALL' : this.shift, timeExport);
      }
      this.dataExport.push(array);
    });

    const label: Array<string> = ['Shift', 'Time', 'Average'];

    if (this.factory != 'ALL' && this.building == 'ALL') {
      label.unshift('Factory', 'Building');
    } else if (this.factory != 'ALL' && this.building != 'ALL') {
      label.unshift('Factory', 'Building', 'Machine');
    } else {
      label.unshift('Factory');
    }

    this.availabilityService.exportExcel(this.dataExport, label, this.factory, this.building);
  }

  changeFactory(value: any) {
    this.building = 'ALL';
    if (value != 'ALL') {
      this.loadBuilding();
    } else {
      this.loadChart();
    }
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
      // Delete No Shift in Shift List
      let x = res.findIndex(x => x.shift_name.trim() == "No Shift");
      res.splice(x, 1);
      
      this.shifts = res.map(item => {
        return {id: item.shift_id.toString(),text: item.shift_name}
      });
      this.shifts.unshift({id: '0',text: 'All Shifts'});
    });
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
    this.shift = event.toString();
    this.loadChart();
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
    if (value === 'quarter') {
      this.resetDate();
      this.changeQuater(this.quater);
    }
    if (value === 'range') {
      this.resetDate();
      this.loadChart();
    }
  }

  changeWeek(event: any) {
    const time = this.arrayWeek[event - 1];
    this.date = new Date(time.weekStart);
    this.dateTo = new Date(time.weekFinish);
    this.loadChart();
  }

  changeMonth(event: any) {
    this.loadChart();
  }

  changeQuater(event: any) {
    const time = this.commonService.getFirstLastDayOfQuater(event);
    this.date = time.firstDate;
    this.dateTo = time.lastDate;
    this.loadChart();
  }

  // event chose date
  updateDate(event: any) {
    if (formatDate(this.date, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.date = new Date(event.srcElement.value);
      this.dateTo = new Date(event.srcElement.value);
      // let dateConvert = this.utilityService.getDateFormat(new Date(this.date));
      this.loadChart();
    }
  }

  // event chose date to
  updateDateTo(event: any) {
    if (formatDate(this.dateTo, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.dateTo = new Date(event.srcElement.value);
      this.loadChart();
    }
  }

  // Get all building of factory selected
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
      return { id: item.id, text:  item.machine_type_name };
    });
    this.machine_types.unshift({ id: 'ALL', text: 'All Machine Types' });
  }, error => {
    console.log(error);
  });
}
  // Get list weeek of year
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

  // reset date to default
  resetDate() {
    this.date = new Date();
    this.dateTo = new Date();
    this.month = (new Date().getMonth() + 1).toString();
    this.week = this.currentWeek();
    this.quater = (Math.floor((new Date().getMonth() + 3) / 3)).toString();
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
    debugger
    const data = {
      factory: this.factory,
      building: this.building,
      machine_type: this.machine_type,
      shift: this.shift,
      month: this.month,
      date: formatDate(this.date, 'yyyy-MM-dd', 'en-US'),
      dateTo: formatDate(this.dateTo, 'yyyy-MM-dd', 'en-US'),
      // toTime: formatDate(this.dateTo, 'yyyy-MM-dd', 'en-US')
    };
    console.log('-----data param----',data);
    this.dataDashboard.emit(data);
  }
}

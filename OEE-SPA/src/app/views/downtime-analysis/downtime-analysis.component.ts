import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-downtime-analysis',
  templateUrl: './downtime-analysis.component.html'
})
export class DowntimeAnalysisComponent implements OnInit, AfterViewInit {
  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  shift: string = '0';
  date: Date = new Date();

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

  public shifts: Array<Select2OptionData> = [
    {
      id: 'all',
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

  constructor(private commonService: CommonService) { }

  ngOnInit() {
    this.dataChart = [
      {
        title: 'Down Time Reason Analysis',
        data: [51, 40, 30, 20, 10],
        labels: ['Manpower', 'Method', 'Material', 'Machine', 'Others']
      },
      {
        title: 'Down Time Reason Analysis',
        data: [101, 90, 80, 70, 60, 50, 40, 30, 20, 10],
        labels: ['Unplaned Maintenance', 'Quality Issues', 'Production Trial', 'Lack of Material', 'Lack of Manpower', 'Development Trial', 'Change Punching', 'Change Knife', 'Adjust Material', 'Adjust Machine']
      }
    ];
  }

  ngAfterViewInit() {
  }

  changeFactory(value: any) {
    this.building = 'ALL';
    if (value === 'SHB' || value === 'SY2') {
      this.building = value;
    }
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      this.loadBuilding();
    } else {
      this.buildings = null;
      this.machines = null;
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

  changeMachine(event: any): any {

  }

  changeShift() {
    // this.loadChart();
  }

  updateDate(event: any) {
    // tslint:disable-next-line: triple-equals
    if (formatDate(this.date, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.date = new Date(event.srcElement.value);
      // this.loadChart();
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

  loadMachine() {
    this.commonService.getMachine(this.factory, this.building).subscribe(res => {

      this.machines = res.map(item => {
        return { id: item, text: 'Machine ' + item };
      });

      this.machines.unshift({ id: 'ALL', text: 'All Machine' });

    }, error => {
      console.log(error);
    });
  }
}

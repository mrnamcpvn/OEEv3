import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeAnalysisService } from '../../../app/_core/_services/downtime-analysis.service';
// import { base64 } from 'src/assets/libary/exceljs/dist/exceljs';


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
    // tslint:disable-next-line: triple-equals
    if (value != 'ALL') {
      this.loadMachine();
    } else {
      this.machines = null;
    }
    this.loadChart();
  }

  changeMachine(value: any) {
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
  this.downtimeAnalysisService.getDowntimeAnalysis(this.factory, this.building, this.machine, this.shift, formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'))
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
        title: 'Reason 1 Analysis',
       data: labelsA,
       labels: arrayA
       },
       {
         title: 'Reason 2 Analysis',
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

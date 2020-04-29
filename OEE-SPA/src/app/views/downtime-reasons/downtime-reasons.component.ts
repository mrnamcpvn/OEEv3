import { Component, OnInit, TemplateRef, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { BsModalService, BsModalRef} from 'ngx-bootstrap/modal';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeReasonsService } from '../../../app/_core/_services/downtime-reasons.service';
import { ActionTime } from '../../../app/_core/_models/action-time';
import { map } from 'highcharts';
//import { parse } from 'path';
import {parseISO} from 'date-fns';
import { ChartReason } from '../../_core/_models/chart-reason';
import { Pagination } from '../../_core/_models/pagination';


declare var google: any;
@Component({
  selector: 'app-downtime-reasons',
  templateUrl: './downtime-reasons.component.html'
})
export class DowntimeReasonsComponent implements OnInit, AfterViewInit {
  @ViewChild('trendChart', { static: true }) trendChart: ElementRef;

  addForm: FormGroup;
  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  shift: string = '0';
  reason_1: string = '0';
  reason_2: string  = '0';
  reason_note: string = '';
pagination: Pagination = {
  currentPage: 1,
  pageSize: 10,
  totalCount: 0,
  totalPage: 0,
};

modalRef: BsModalRef;
  date: Date = new Date();
  dataActionTime: Array<ChartReason> = [];
  dataReason: Array<string[]> = [];
  modal:  ChartReason;
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
  public reason1s: Array<Select2OptionData>;
  public reason2s:  Array<Select2OptionData>;
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

  constructor( private modalService: BsModalService,
    private fb: FormBuilder,
    private commonService: CommonService,
              private downtimeReasonsService: DowntimeReasonsService
             ) { }

  ngOnInit() {
    this.loadChart();
   this.loadReason1(this.reason_1, false);
   this.addForm = this.fb.group({
     reason_1: [],
     reason_2: [],
     reason_note: []
   })
  }
  openModal(template: TemplateRef<any>, item: ChartReason) {
    this.modalRef = this.modalService.show(template);
    this.modal = item;
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
  changeReason(event: any) {
    debugger
    this.loadReason1(this.reason_1, true);
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
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
  loadReason1(value, changed) {
    this.downtimeReasonsService.getDowntimeReasonDetail(value).subscribe(res => {
      if (changed === false) { this.reason1s = res.map(item => {
        return { id: item, text: item };
      });
    } else if (changed === true) { this.reason2s = res.map(item => {
      return { id: item, text: item };
    });
   }
    }, error => {
      console.log(error);
    });
  }

  loadChart() {
    // tslint:disable-next-line: max-line-length
    console.log('f: ' + this.factory + ' m: ' + this.machine + ' s: ' + this.shift + ' d: ' + formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'));
    this.downtimeReasonsService.getDowntimeReasons(this.factory, this.building, this.machine, this.shift, formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'), this.pagination.currentPage)
    .subscribe(res => {
        this.dataActionTime = res.result;
        this.pagination = res.pagination;
        this.drawChart(res.resultC);
        console.log(res);
      }, (error: any) => {

        console.log(error);
      });
  }

  reasonSave() {
    this.modal.reason_1 = this.addForm.value.reason_1;
    this.modal.reason_2 = this.addForm.value.reason_2;
    this.modal.reason_note = this.addForm.value.reason_note;
  //  this.modal.building_id = this.addForm.value.building_id;
    this.downtimeReasonsService.addDowntimeReason(this.modal)
    .subscribe(res => {
        console.log(res);
      }, (error: any) => {

        console.log(error);
      });
  }
  ngAfterViewInit() {
    google.charts.load('current', { packages: ['timeline'] });
    google.charts.setOnLoadCallback(this.drawChart);
  }

  onResize(event: any) {
    google.charts.load('current', { packages: ['timeline'] });
    google.charts.setOnLoadCallback(this.drawChart);
  }
  drawChart = (result) => {
    const arrayA = result.map(function(item) {
        return [item['title'].toString(),
                parseISO(item['start_time']) , parseISO(item['end_time'])];
    });
    const data = google.visualization.arrayToDataTable(arrayA);

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

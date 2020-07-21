import { Component, OnInit, TemplateRef, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Select2OptionData } from 'ng-select2';
import { BsModalService, BsModalRef} from 'ngx-bootstrap/modal';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Options } from 'select2';
import { CommonService } from '../../_core/_services/common.service';
import { formatDate } from '@angular/common';
import { DowntimeReasonsService } from '../../../app/_core/_services/downtime-reasons.service';
import { map } from 'highcharts';
import Swal from 'sweetalert2';

import {parseISO} from 'date-fns';
import { ChartReason } from '../../_core/_models/chart-reason';
import { Pagination } from '../../_core/_models/pagination';
import * as moment from 'moment';


declare var google: any;
@Component({
  selector: 'app-downtime-reasons',
  templateUrl: './downtime-reasons.component.html'
})
export class DowntimeReasonsComponent implements OnInit, AfterViewInit {
  @ViewChild('trendChart', { static: false  }) trendChart: ElementRef;

  addForm: FormGroup;
  factory: string = 'ALL';
  building: string = 'ALL';
  machine: string = 'ALL';
  machineName: string = '';
  machine_type: string = 'ALL';
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
  factories: Array<Select2OptionData> = [];
  buildings: Array<Select2OptionData>;
  machine_types: Array<Select2OptionData>;
  public shifts: Array<Select2OptionData> = [];
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
  constructor(  private modalService: BsModalService,
                private fb: FormBuilder,
                private commonService: CommonService,
                private downtimeReasonsService: DowntimeReasonsService ) { }

  ngOnInit() {
    this.getListFactory();
    this.getListShift();
    google.charts.load('current', { packages: ['timeline'] });
    this.loadReason1(this.reason_1, false);
    this.addForm = this.fb.group({
      reason_1: [],
      reason_2: [],
      reason_note: []
    });
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
  openModal(template: TemplateRef<any>, item: ChartReason) {
    this.downtimeReasonsService.getReasons(item.id)
    .subscribe(res => {
    if(res != null)
    {
      this.reason_1 = res.reason_1;
      this.reason_2 = res.reason_2;
      this.reason_note = res.reason_note;
    }
      else
      {
        this.reason_1 = '';
        this.reason_2 = '';
        this.reason_note = '';
      }
    });
    this.modalRef = this.modalService.show(template);
    this.modal = item;
  }
  changeFactory(value: any) {
    debugger
    this.building = 'ALL';
    if (value != 'ALL') {
      this.loadBuilding();
    } else {
      this.buildings = [];
      this.machines = [];
      this.machine_types = [];
    }
  }
  changeMachine_Type(event: any) {
    this.loadMachine();
    this.loadChart();
  }
  changeMachine(value: any): any {
    this.machine = value;
    this.loadChart();
  }

  changeShift(event: any) {
    this.loadChart();
  }
  changeReason(event: any) {
    this.loadReason1(this.reason_1, true);
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadChart();
  }
  updateDate(event: any) {
    if (formatDate(this.date, 'yyyy-MM-dd', 'en-US') != formatDate(new Date(event.srcElement.value), 'yyyy-MM-dd', 'en-US')) {
      this.date = new Date(event.srcElement.value);
      this.loadChart();
    }
  }
  changeBuilding(value: any) {
    this.machine_type = "ALL";
    if(value !== "ALL") {
      this.loadMachine_Type();
    } else {
      this.loadChart();
    }
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
  loadMachine() {
    this.commonService.getMachinesActionTime(this.factory, this.building, this.machine_type).subscribe(res => {
      this.machines = res.map(item => {
        return { id: item.machine_id, text: item.machine_id };
      });
      this.machines.unshift({ id: 'ALL', text: 'All Machine' });
    }, error => {
      console.log(error);
    });
  }
  loadReason1(value, changed) {
    this.downtimeReasonsService.getDowntimeReasonDetail(value).subscribe(res => {
      if (res != null)  {
      if (changed === false) { this.reason1s = res.map(item => {
        return { id: item, text: item };
      });
    } else if (changed === true) { this.reason2s = res.map(item => {
      return { id: item, text: item };
    });
   }
  }
    }, error => {
      console.log(error);
    });
  }

  loadChart() {
    this.downtimeReasonsService.getDowntimeReasons(this.factory, this.building, this.machine, this.machine_type, this.shift, formatDate(new Date(this.date), 'yyyy-MM-dd', 'en-US'), this.pagination.currentPage)
    .subscribe(res => {
      console.log(res);
      if (res !== null) {
        debugger
        if (res.result.length > 0) {
          this.dataActionTime = res.result;
          this.pagination = res.pagination;
          this.machineName = res.machineName;
        }
        if (res.resultC.length > 0) {
          document.getElementById("chartActionTime").style.display = "block";
          google.charts.setOnLoadCallback(this.drawChart(res.resultC));
        }
      } else {
        document.getElementById("chartActionTime").style.display = "none";
        this.dataActionTime = [];
      }
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
        if (res === true) {
          this.modalRef.hide();
          Swal.fire('Saved!', 'Your change has been saved.', 'success');
          this.loadChart();
        }
      }, (error: any) => {
        Swal.fire('Oops...', 'Something went wrong!', 'error');
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
   extractTime(value: string) {
    const year =  moment(value).format('YYYY');
    const month =  moment(value).format('MM');
    const day =  moment(value).format('DD');
    const hour =  moment(value).format('HH');
    const minute =  moment(value).format('mm');
    const second =  moment(value).format('ss');
    return new Date(
    parseInt(year, 10), parseInt(month, 10), parseInt(day, 10),
    parseInt(hour, 10), parseInt(minute, 10),
    parseInt(second, 10)
    );
  }
  drawChart = (result) => {
    if (result != null) {
    const arrayA = result.map((item: any) => {
        const start = this.extractTime(item['start_time']);
        const  end = this.extractTime(item['end_time']);
          return [
            item['title'],
            start,
            end ];
    });
    arrayA.unshift( ['title', 'date1', 'date2']);
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

}

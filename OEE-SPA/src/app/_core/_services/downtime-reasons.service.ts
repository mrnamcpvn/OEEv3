import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chart_trend } from '../_models/chart_trend';
import { ActionTime } from '../_models/action-time';
import { maps, chart } from 'highcharts';
import {map} from 'rxjs/operators';
import { ChartReason } from '../_models/chart-reason';
import { PaginationResult } from '../_models/pagination';
import { Downtimereason } from '../_models/downtime_reason';

@Injectable({
  providedIn: 'root'
})
export class DowntimeReasonsService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // // tslint:disable-next-line: max-line-length
  // getDowntimeReasons(factory: string, machine: string, shift: string, date: string): Observable<{ dataChart, listTime: string[] }> {
  //   // tslint:disable-next-line: max-line-length
  //   return this.http.get<{ dataChart, listTime: string[] }>(this.baseUrl + '
  // 'DowntimeReasons?factory=' + factory + '&machine=' + machine + '&shift=' + shift + '&date=' + date);
  // }
  getDowntimeReasonDetail(reason_1: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'DowntimeReasons/getDowntimeReasonDetail?reason_1=' + reason_1);
  }
  addDowntimeReason(chartreason: ChartReason): Observable<any> {
    return this.http.post<ChartReason>(this.baseUrl + 'DowntimeReasons/addDowntimeReason', chartreason);
  }
    // tslint:disable-next-line: max-line-length
    getDowntimeReasons(factory: string, building: string, machine: string, shift: string, date: string, page: number): Observable<PaginationResult<ChartReason[]>> {
      // tslint:disable-next-line: max-line-length
      return this.http.get<PaginationResult<ChartReason[]>>(this.baseUrl + 'DowntimeReasons/getDowntimeReasons?factory=' + factory + '&building=' + building + '&machine=' + machine + '&shift=' + shift + '&date=' + date + '&page=' + page);
    }
    getReasons(item: number): Observable<Downtimereason> {
      return this.http.get<Downtimereason>(this.baseUrl + 'DowntimeReasons/getReasons?item=' + item);
    }
}

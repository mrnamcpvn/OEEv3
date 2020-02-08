import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chart_trend } from '../_models/chart_trend';
import { ActionTime } from '../_models/action-time';

@Injectable({
  providedIn: 'root'
})
export class DowntimeReasonsService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // // tslint:disable-next-line: max-line-length
  // getDowntimeReasons(factory: string, machine: string, shift: string, date: string): Observable<{ dataChart, listTime: string[] }> {
  //   // tslint:disable-next-line: max-line-length
  //   return this.http.get<{ dataChart, listTime: string[] }>(this.baseUrl + 'DowntimeReasons?factory=' + factory + '&machine=' + machine + '&shift=' + shift + '&date=' + date);
  // }

    // tslint:disable-next-line: max-line-length
    getDowntimeReasons(factory: string, machine: string, shift: string, date: string): Observable<ActionTime[]> {
      // tslint:disable-next-line: max-line-length
      return this.http.get<ActionTime[]>(this.baseUrl + 'DowntimeReasons?factory=' + factory + '&machine=' + machine + '&shift=' + shift + '&date=' + date);
    }
}

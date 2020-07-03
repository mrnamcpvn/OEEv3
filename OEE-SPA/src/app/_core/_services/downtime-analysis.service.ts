import { Injectable } from '@angular/core';
import { ChartReason } from '../_models/chart-reason';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReasonAnalysis } from '../_models/reason-analysis';

@Injectable({
  providedIn: 'root'
})
export class DowntimeAnalysisService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
// tslint:disable-next-line: max-line-length
getDowntimeAnalysis(factory: string, building: string, machine_type: string, machine: string, shift: string, date: string, dateTo: string): Observable<any> {
  // tslint:disable-next-line: max-line-length
  return this.http.get<any>(this.baseUrl + 'DowntimeAnalysis/getDowntimeAnalysis?factory=' + factory + '&building=' + building + '&machine_type=' + machine_type + '&machine=' + machine + '&shift=' + shift + '&date=' + date + '&dateTo=' + dateTo);
}
}

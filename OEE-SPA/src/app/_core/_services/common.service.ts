import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Week } from '../_models/week';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getBuilding(factory: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'Common/' + factory);
  }

  getMachine(factory: string, building: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'Common/' + factory + '/GetMachine/' + building);
  }

  getWeeks(): Observable<Week[]> {
    return this.http.get<Week[]>(this.baseUrl + 'Common/WeekInYear');
  }

  getBuildingsActionTime(factory: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'Common/GetAllBuildingActionTime/' + factory);
  }

  getMachinesActionTime(factory: string, building: string): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'Common/GetAllMachineActionTime?factory=' + factory + '&building=' + building);
  }

  getFistLastDayOfMonth(month: number) {
    const date = new Date();

    const firstDate = new Date(date.getFullYear(), month - 1, 1);

    const lastDate = new Date(date.getFullYear(), month, 0);

    return {firstDate: firstDate, lastDate: lastDate};
  }

  getFirstLastDayOfQuater(quarter: string) {

    const now = new Date();
    let month = 1;
    if (quarter === '1') {
      month = 1;
    }
    if (quarter === '2') {
      month = 4;
    }
    if (quarter === '3') {
      month = 7;
    }
    if (quarter === '4') {
      month = 10;
    }

    const firstDate = new Date(now.getFullYear(), month - 1, 1);

    const lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);

    return {firstDate: firstDate, lastDate: lastDate};
  }
}

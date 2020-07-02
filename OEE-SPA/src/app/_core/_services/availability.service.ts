import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Chart_availability } from '../_models/chart_availability';
import { Row, Workbook, Worksheet } from '../../../assets/libary/exceljs';
import * as fs from 'file-saver';
import { formatDate } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AvailabilityService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: max-line-length
  getAvailability(factory: string, building: string, machine_type: string, shift: string, date: string, dateTo: string): Observable<Chart_availability[]> {
    // tslint:disable-next-line: max-line-length
    return this.http.get<Chart_availability[]>(this.baseUrl + 'Availability/GetAvailability?factory=' + factory + '&building=' + building + '&machine_type=' + machine_type + '&shift=' + shift + '&date=' + date + '&dateTo=' + dateTo);
  }

  exportExcel(dataExport: Array<Array<string>>, labels: Array<string>, factory: string, building: string) {
    // Excel Title, Header, Data
    const title = 'AVAILABILITY';
    const header = labels;
    const data = dataExport;
    const arrayColumnExcel = 'abcdefghijklmnopqrstuvwxyz'.toUpperCase().split('');

    // Create workbook and worksheet
    const workbook: Workbook = new Workbook();
    const worksheet: Worksheet = workbook.addWorksheet('Availability');

    // Add Row and formatting
    const titleRow = worksheet.addRow([title]);
    titleRow.font = { size: 18, bold: true };
    titleRow.getCell(1).alignment = { vertical: 'middle', horizontal: 'center' };
    worksheet.addRow([]);
    const subTitleRow = worksheet.addRow(['Date : ' + formatDate(new Date(), 'yyyy/MM/dd', 'en-US')]);

    // merge cell title
    worksheet.mergeCells(`A${titleRow.number}:${arrayColumnExcel[header.length - 1]}${titleRow.number + 1}`);
    worksheet.mergeCells(`A${subTitleRow.number}:${arrayColumnExcel[header.length - 1]}${subTitleRow.number}`);

    const headerRow = worksheet.addRow(header);

    // Cell Style : Fill and Border
    headerRow.eachCell((cell, index) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFFFFF00' },
        bgColor: { argb: 'FFFF' }
      };
      cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      cell.font = { bold: true };
      cell.alignment = { vertical: 'middle', horizontal: 'center' };
    });

    for (let i = 0; i < data.length; i++) {

      const row = worksheet.addRow(data[i]);

      row.eachCell((cell, number) => {
        cell.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'FFFFFFFF' },
          bgColor: { argb: 'FFFFFFFF' }
        };
        cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
      });
    }

    for (let i = 0; i < worksheet.columns.length; i += 1) {
      let dataMax = 0;
      const column = worksheet.columns[i];
      for (let j = 1; j < column.values.length; j += 1) {
        const columnLength = column.values[j].toString().length;
        if (columnLength > dataMax) {
          dataMax = columnLength;
        }
      }
      column.width = dataMax < 8 ? 8 : dataMax;
    }

    // Generate Excel File with given name
    // tslint:disable-next-line:no-shadowed-variable
    workbook.xlsx.writeBuffer().then((data: any) => {
      const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      fs.saveAs(blob, 'Availability_' + formatDate(new Date(), 'ddMMyyy', 'en-US') + '.xlsx');
    });
  }
}

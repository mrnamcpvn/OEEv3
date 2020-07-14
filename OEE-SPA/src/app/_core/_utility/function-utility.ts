import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class FunctionUtility {
    /**
     *Hàm tiện ích
     */


    constructor() {
    }

    /**
     *Trả ra ngày hiện tại, chỉ lấy năm tháng ngày: yyyy/MM/dd
     */
    getToDay() {
        const toDay = new Date().getFullYear().toString() +
            '/' + (new Date().getMonth() + 1).toString() +
            '/' + new Date().getDate().toString();
        return toDay;
    }

    /**
     *Trả ra ngày với tham số truyền vào là ngày muốn format, chỉ lấy năm tháng ngày: yyyy/MM/dd
     */
    getDateFormat(day: Date) {
        const dateFormat = day.getFullYear().toString() +
            '/' + (day.getMonth() + 1).toString() +
            '/' + day.getDate().toString();
        return dateFormat;
    }

   
}

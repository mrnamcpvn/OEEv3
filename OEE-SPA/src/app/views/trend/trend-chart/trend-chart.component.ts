import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, HostListener, Input, OnChanges } from '@angular/core';
import { CustomTooltips } from '@coreui/coreui-plugin-chartjs-custom-tooltips';

@Component({
    selector: 'app-trend-chart',
    templateUrl: 'trend-chart.component.html'
})

export class TrendChartComponent implements OnInit, AfterViewInit, OnChanges {
    // tslint:disable-next-line: no-input-rename
    @Input('title') title: string;
    // tslint:disable-next-line:no-input-rename
    @Input('data') data: Array<{ label: string, data: Array<number> }>;
    // tslint:disable-next-line: no-input-rename
    @Input('labels') labels: Array<string>;

    public trendChart: any;
    // tslint:disable-next-line: max-line-length
    public dataset = [];
    public colors = ['Red', 'Blue', 'Orange',  'Green', 'Purple', 'Indigo', 'Navy', 'Teal', 'Brown', 'Lime', 'Gray', 'Pink'];
    constructor() { }

    ngOnInit(): void {
    }

    ngAfterViewInit() {
    }

    ngOnChanges() {
        this.dataset = [];
        this.data.forEach((item, index) => {
            // const r = Math.floor((Math.random() * 255));
            // const g = Math.floor((Math.random() * 255));
            // const b = Math.floor((Math.random() * 255));
            // const rgbStr = r + ', ' + g + ', ' + b;

            this.dataset.push({
                label: item.label,
                data: item.data,
                borderColor: this.colors[index],
                pointBorderColor: 'white',
                pointBackgroundColor: this.colors[index],
                pointHoverBackgroundColor: 'white',
                pointHoverBorderColor: this.colors[index],
            });
        });
        this.trendChart = {
            type: 'line',
            data: this.dataset,
            labels: this.labels,
            options: {

                title: {
                    display: true,
                    text: 'Trend of Histories'
                },
                tooltips: {
                    enabled: false,
                    custom: CustomTooltips,
                    intersect: true,
                    mode: 'index',
                    position: 'nearest',
                },
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    xAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'Time'
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                            stepSize: 25,
                            min: 0,
                            max: 100
                        },
                        scaleLabel: {
                            display: true,
                            labelString: 'Trend of Factories'
                        }
                    }]
                },
                elements: {
                    line: {
                        borderWidth: 2,
                        fill: false
                    },
                    point: {
                        radius: 5,
                        hitRadius: 10,
                        hoverRadius: 4,
                        hoverBorderWidth: 3,
                    },
                },
                legend: {
                    position: 'bottom',
                    labels: {
                        fontSize: 12,
                        boxWidth: 12,
                        // usePointStyle: true,
                    }
                }
            }
        };
    }
}




// this.trendChart = {
//     type: 'line',
//     data: [
//         {
//             data: [73, 77, 55, 75, 71, 60],
//             pointRadius: 8,
//             pointStyle: 'triangle',
//             fill: false,
//             borderColor: 'red',
//             pointBorderColor: 'white',
//             pointBackgroundColor: 'red'
//         },
//         {
//             data: [79, 85, 78, 65, 79, 25],
//             pointRadius: 8,
//             pointStyle: 'circle',
//             borderColor: 'blue',
//             pointBorderColor: 'white',
//             pointBackgroundColor: 'blue',
//             fill: false,
//         },
//         {
//             data: [0, 77, 14, 77, 76, 41],
//             pointRadius: 8,
//             pointStyle: 'rectRot',
//             borderColor: 'black',
//             pointBorderColor: 'white',
//             pointBackgroundColor: 'black',
//             fill: false,
//         },
//         {
//             data: [79, 45, 89, 23, 82, 82],
//             pointRadius: 8,
//             pointStyle: 'rectRounded',
//             borderColor: 'yellow',
//             pointBorderColor: 'white',
//             pointBackgroundColor: 'yellow',
//             fill: false,
//         }
//     ],
//     series: ['SHW', 'SHD', 'SHB', 'SY2'],
//     labels: ['2006', '2007', '2008', '2009', '2010', '2011'],
//     options: {
//         title: {
//             display: true,
//             text: 'Trend of Histories'
//         },
//         tooltips: {
//             enabled: false,
//             custom: CustomTooltips,
//             intersect: true,
//             mode: 'index',
//             position: 'nearest',
//         },
//         responsive: true,
//         maintainAspectRatio: false,
//         scales: {
//             xAxes: [{
//                 scaleLabel: {
//                     display: true,
//                     labelString: 'Time'
//                 }
//             }],
//             yAxes: [{
//                 ticks: {
//                     beginAtZero: true,
//                     stepSize: 25,
//                     min: 0,
//                     max: 100
//                 },
//                 scaleLabel: {
//                     display: true,
//                     labelString: 'Trend of Factories'
//                 }
//             }]
//         },
//         elements: {
//             line: {
//                 borderWidth: 2
//             },
//             point: {
//                 radius: 0,
//                 hitRadius: 10,
//                 hoverRadius: 4,
//                 hoverBorderWidth: 3,
//             }
//         },
//         legend: {
//             position: 'bottom',
//             labels: {
//                 fontSize: 12,
//                 boxWidth: 12,
//                 usePointStyle: true,
//             }
//         }
//     }

// };

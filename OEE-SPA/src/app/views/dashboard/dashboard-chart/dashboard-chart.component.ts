import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, HostListener, Input } from '@angular/core';
import * as Highcharts from 'highcharts';
declare var $: any;
@Component({
    selector: 'app-dashboard-chart',
    templateUrl: 'dashboard-chart.component.html'
})

export class DashboardChartComponent implements OnInit, AfterViewInit {
    @ViewChild('chartGauge', { static: true }) chartGauge: ElementRef;
    @ViewChild('sevenSeg', { static: true }) sevenSeg: ElementRef;
    // tslint:disable-next-line: no-input-rename
    @Input('name') name: string;
    // tslint:disable-next-line:no-input-rename
    @Input('data') data: number;
    // tslint:disable-next-line:no-input-rename
    @Input('color-on') colorOn: string;
    // tslint:disable-next-line:no-input-rename
    @Input('color-off') colorOff: string;

    public optionsChart: any;

    ngOnInit(): void {
    }

    ngAfterViewInit() {
        this.optionsChart = {
            chart: {
                type: 'gauge',
                plotBackgroundColor: null,
                plotBackgroundImage: null,
                plotBorderWidth: 0,
                plotShadow: false
            },
            title: {
                text: ''
            },
            pane: {
                startAngle: -150,
                endAngle: 150,
                background: [{
                    backgroundColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, '#FFF'],
                            [1, '#333']
                        ]
                    },
                    borderWidth: 0,
                    outerRadius: '109%'
                }, {
                    backgroundColor: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0, '#333'],
                            [1, '#FFF']
                        ]
                    },
                    borderWidth: 1,
                    outerRadius: '107%'
                }, {
                    // default background
                }, {
                    backgroundColor: '#DDD',
                    borderWidth: 0,
                    outerRadius: '105%',
                    innerRadius: '103%'
                }]
            },

            // the value axis
            yAxis: {
                min: 0,
                max: 100,

                minorTickInterval: 'auto',
                minorTickWidth: 1,
                minorTickLength: 10,
                minorTickPosition: 'inside',
                minorTickColor: '#666',

                tickPixelInterval: 30,
                tickWidth: 2,
                tickPosition: 'inside',
                tickLength: 10,
                tickColor: '#666',
                labels: {
                    step: 2,
                    rotation: 'auto'
                },
                title: {
                    text: this.name + '(%)'
                },
                plotBands: [{
                    from: 0,
                    to: 60,
                    color: '#ED0000'
                }, {
                    from: 60,
                    to: 85,
                    color: '#fff202'
                }, {
                    from: 85,
                    to: 100,
                    color: '#55BF3B'
                }]
            },
            credits: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: 'Availability',
                data: [this.data],
                tooltip: {
                    valueSuffix: ' %'
                }
            }]
        };

        Highcharts.chart(this.chartGauge.nativeElement, this.optionsChart);
        $(this.sevenSeg.nativeElement).sevenSeg({
            digits: 3,
            value: this.data,
            colorOn: this.colorOn,
            colorOff: this.colorOff
        });
    }
}

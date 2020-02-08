import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, HostListener, Input } from '@angular/core';

@Component({
    selector: 'app-analysis-chart',
    templateUrl: 'analysis-chart.component.html'
})

export class AnalysisChartComponent implements OnInit, AfterViewInit {
    // tslint:disable-next-line: no-input-rename
    @Input('title') title: string;
    // tslint:disable-next-line:no-input-rename
    @Input('data') data: Array<number>;
    // tslint:disable-next-line: no-input-rename
    @Input('labels') labels: Array<number>;

    public barChart: any;

    ngOnInit(): void {
        this.barChart = {
            type: 'horizontalBar',
            labels: this.labels,
            data: [
                {
                    data: this.data,
                    label: 'Hours per Day',
                    backgroundColor: '#1553de',
                    borderColor: '#6d6d6d',
                    hoverBackgroundColor: '#1553de',
                    hoverBorderWidth: 2,
                    hoverBorderColor: '#6d6d6d',
                    minHeight: 2
                }
            ],
            options: {
                title: {
                    display: true,
                    text: this.title
                },
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    xAxes: [{
                        display: true,
                        ticks: {
                            min: 0
                        }
                    }]
                }
            }
        };
    }

    ngAfterViewInit() {

    }
}

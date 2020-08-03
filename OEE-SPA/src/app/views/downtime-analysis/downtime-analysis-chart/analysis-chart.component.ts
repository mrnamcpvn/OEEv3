import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, HostListener, Input } from '@angular/core';

@Component({
    selector: 'app-analysis-chart',
    templateUrl: 'analysis-chart.component.html'
})

export class AnalysisChartComponent implements OnInit, AfterViewInit {
    @Input('title') title: string;
    @Input('data') data: Array<number>;
    @Input('labels') labels: Array<number>;

    public barChart: any;

    ngOnInit(): void {
        this.barChart = {
            type: 'horizontalBar',
            labels: this.labels,
            data: [
                {
                    data: this.data,
                    label: 'Total Minues',
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
                    text: this.title,
                    fontSize: 18,
                    fontColor: '#000000',
                    bold: true
                },
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    xAxes: [{
                        display: true,
                        ticks: {
                            offset: true,
                            beginAtZero: true
                        }
                    }],
                },
                vAxis: [{
                    ticks: {
                        offset: true
                    }
                }]
            }
        };
    }

    ngAfterViewInit() {

    }
}

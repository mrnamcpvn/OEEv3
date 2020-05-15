/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { DowntimeAnalysisService } from './downtime-analysis.service';

describe('Service: DowntimeAnalysis', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DowntimeAnalysisService]
    });
  });

  it('should ...', inject([DowntimeAnalysisService], (service: DowntimeAnalysisService) => {
    expect(service).toBeTruthy();
  }));
});

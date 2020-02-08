/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { DowntimeReasonsService } from './downtime-reasons.service';

describe('Service: DowntimeReasons', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DowntimeReasonsService]
    });
  });

  it('should ...', inject([DowntimeReasonsService], (service: DowntimeReasonsService) => {
    expect(service).toBeTruthy();
  }));
});

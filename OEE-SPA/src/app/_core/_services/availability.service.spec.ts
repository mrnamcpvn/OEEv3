/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AvailabilityService } from './availability.service';

describe('Service: Availability', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AvailabilityService]
    });
  });

  it('should ...', inject([AvailabilityService], (service: AvailabilityService) => {
    expect(service).toBeTruthy();
  }));
});

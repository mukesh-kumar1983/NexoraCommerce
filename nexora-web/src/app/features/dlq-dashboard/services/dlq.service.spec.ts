import { TestBed } from '@angular/core/testing';

import { DlqService } from './dlq.service';

describe('DlqService', () => {
  let service: DlqService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DlqService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

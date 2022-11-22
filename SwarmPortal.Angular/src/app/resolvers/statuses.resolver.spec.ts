import { TestBed } from '@angular/core/testing';

import { StatusesResolver } from './statuses.resolver';

describe('StatusesResolver', () => {
  let resolver: StatusesResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(StatusesResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

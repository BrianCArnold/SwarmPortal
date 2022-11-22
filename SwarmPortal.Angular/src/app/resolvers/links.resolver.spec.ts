import { TestBed } from '@angular/core/testing';

import { LinksResolver } from './links.resolver';

describe('LinksResolver', () => {
  let resolver: LinksResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LinksResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

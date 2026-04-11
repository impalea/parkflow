import { TestBed } from '@angular/core/testing';

import { PriceConfigService } from './price-config';

describe('PriceConfigService', () => {
	let service: PriceConfigService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(PriceConfigService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});

import { TestBed } from '@angular/core/testing';

import { ParkingSpot } from './parking-spot';

describe('ParkingSpot', () => {
	let service: ParkingSpot;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(ParkingSpot);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});

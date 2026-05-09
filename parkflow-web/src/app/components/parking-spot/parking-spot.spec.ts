import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParkingSpot } from './parking-spot';

describe('ParkingSpot', () => {
	let component: ParkingSpot;
	let fixture: ComponentFixture<ParkingSpot>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ParkingSpot],
		}).compileComponents();

		fixture = TestBed.createComponent(ParkingSpot);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});

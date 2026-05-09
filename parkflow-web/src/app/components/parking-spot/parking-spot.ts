import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ParkingSpotService, Spot } from '../../services/parking-spot/parking-spot';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
	selector: 'app-parking-spot',
	standalone: true,
	imports: [CommonModule, FormsModule],
	templateUrl: './parking-spot.html',
	styleUrl: './parking-spot.scss',
})
export class ParkingSpot implements OnInit {
	spots = signal<Spot[]>([]);
	isLoading = signal(false);

	newSpotNumber = signal('');

	constructor(
		private parkingSpotService: ParkingSpotService,
		private router: Router
	) { }

	ngOnInit(): void {
		this.loadParkingSpots();
	}

	loadParkingSpots(): void {
		this.parkingSpotService.getAll().subscribe({
			next: (data) => {
				this.spots.set(data);
			},
			error: (err) => {
				console.error('Error fetching parking spots:', err);
			}
		});
	}

	createSpot(): void {
		const spotNumber = this.newSpotNumber().trim();
		if (!spotNumber) return;

		this.isLoading.set(true);

		this.parkingSpotService.create({ spotNumber }).subscribe({
			next: (newSpot) => {
				this.newSpotNumber.set('');
				this.loadParkingSpots();
			},
			error: (err) => {
				console.error('Error creating parking spot:', err);
				this.isLoading.set(false);
			},
			complete: () => {
				this.isLoading.set(false);
			}
		});
	}

	deleteSpot(id: number): void {
		if (!confirm('Are you sure you want to delete this parking spot?')) return;

		this.parkingSpotService.delete(id).subscribe({
			next: () => {
				this.loadParkingSpots();
			},
			error: (err) => {
				console.error('Error deleting parking spot:', err);
			}
		});
	}

	goBack() {
		this.router.navigate(['/settings']);
	}
}

import { Component, computed, OnInit, signal } from '@angular/core';
import { ParkingSpot, ParkingSpotDashboard } from '../../services/parking-spot/parking-spot';
import { CheckOutPreview, Ticket } from '../../services/ticket/ticket';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
	selector: 'app-dashboard',
	standalone: true,
	imports: [CommonModule, FormsModule],
	templateUrl: './dashboard.html',
	styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
	spots = signal<ParkingSpotDashboard[]>([]);

	totalSpots = computed(() => this.spots().length);

	totalOccupied = computed(() =>
  	this.spots().filter(s => s.isOccupied).length
	);

	totalAvailable = computed(() =>
  	this.spots().filter(s => !s.isOccupied).length
	);

	selectedSpot = signal<ParkingSpotDashboard | null>(null);
	checkOutData = signal<CheckOutPreview | null>(null);

	isCheckInModalOpen = signal(false);
	isCheckOutModalOpen = signal(false);

	currentTicketId = 0;
	checkInForm = {
		licensePlate: '',
		model: '',
		color: '',
		parkingSpotId: 0
	}

	constructor(
		private parkingSpotService: ParkingSpot,
		private ticketService: Ticket
	) { }

	ngOnInit(): void {
		this.loadDashboard();
	}

	loadDashboard(): void {
		this.parkingSpotService.getDashboard().subscribe({
			next: (data) => {
				this.spots.set(data);
			},
			error: (err) => {
				console.error('Error fetching parking spot dashboard:', err);
			}
		});
	}

	doCheckIn(spot: ParkingSpotDashboard): void {
		this.selectedSpot.set(spot);
		this.isCheckInModalOpen.set(true);
	}

	createCheckIn() {
		const spot = this.selectedSpot();
		if (!spot) return;

		const cleanPlate = this.checkInForm.licensePlate
		.toUpperCase()
		.replace(/[^A-Z0-9]/g, '');

		const payload = {
			licensePlate: cleanPlate,
			model: this.checkInForm.model,
			color: this.checkInForm.color,
			parkingSpotId: spot.parkingSpotId
		};

		this.ticketService.createCheckIn(payload).subscribe({
			next: () => {
				this.closeCheckInModal();
				this.loadDashboard();
			},
			error: (err) => {
				console.error('Error during check-in:', err);
				alert('Falha ao registrar entrada.');
			}
		});
	}

	doCheckOut(spot: ParkingSpotDashboard) {
		if (!spot.ticketId) return;

		this.currentTicketId = spot.ticketId;

		this.ticketService.getCheckOutPreview(spot.ticketId).subscribe({
			next: (data) => {
				this.checkOutData.set(data);
				this.isCheckOutModalOpen.set(true);
			},
			error: (err: any) => {
				console.error('Error fetching check-out preview:', err);
			}
		});
	}

	confirmCheckOut() {
		if (!this.currentTicketId) return;

		this.ticketService.confirmCheckOut(this.currentTicketId).subscribe({
			next: () => {
				this.isCheckOutModalOpen.set(false);
				this.loadDashboard();
			},
			error: (err: any) => {
				console.error('Error confirming check-out:', err);
				alert('Falha ao finalizar check-out.')
			}
		});
	}

	closeCheckInModal() {
		this.isCheckInModalOpen.set(false);
		this.selectedSpot.set(null);
		this.checkInForm = { licensePlate: '', model: '', color: '', parkingSpotId: 0 };
	}

	closeCheckoutModal() {
		this.isCheckOutModalOpen.set(false);
		this.checkOutData.set(null);
		this.currentTicketId = 0;
	}
}



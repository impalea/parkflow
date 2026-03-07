import { Component, OnInit, signal } from '@angular/core';
import { ParkingSpot, ParkingSpotDashboard } from '../../services/parking-spot';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-dashboard',
	standalone: true,
	imports: [CommonModule],
	templateUrl: './dashboard.html',
	styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
	spots = signal<ParkingSpotDashboard[]>([]);

	constructor(private parkingSpotService: ParkingSpot) { }

	ngOnInit(): void {
		this.parkingSpotService.getDashboard().subscribe({
			next: (data) => {
				this.spots.set(data);
			},
			error: (error) => {
				console.error('Error fetching parking spot dashboard:', error);
			}
		});
	}

	doCheckIn(spot: ParkingSpotDashboard) {
		console.log('Iniciando Check-in para a vaga:', spot.spotNumber);
		// No futuro, aqui abrirá um modal ou formulário
		alert(`Abrir formulário de entrada para a vaga ${spot.spotNumber}`);
	}

	doCheckOut(spot: ParkingSpotDashboard) {
		console.log('Iniciando Check-out para o ticket:', spot.ticketId);
		// No futuro, aqui abrirá um modal ou formulário
		alert(`Confirmar saída do veículo ${spot.licensePlate}`);
	}
}



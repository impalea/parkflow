import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ParkingSpotDashboard {
	parkingSpotId: number;
	spotNumber: string;
	isOccupied: boolean;
	ticketId?: number;
	licensePlate?: string;
	model?: string;
	color?: string;
	entryTime?: string;
}

@Injectable({
	providedIn: 'root'
})
export class ParkingSpot {
	private apiUrl = 'http://localhost:5115/api/parkingspot';

	constructor(private http: HttpClient) { }

	getDashboard(): Observable<ParkingSpotDashboard[]> {
		return this.http.get<ParkingSpotDashboard[]>(`${this.apiUrl}/dashboard`);
	}
}

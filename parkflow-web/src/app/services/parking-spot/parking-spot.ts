import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

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

export interface Spot {
	id: number;
	spotNumber: string;
	isOccupied: boolean;
}

@Injectable({
	providedIn: 'root'
})
export class ParkingSpotService {
	private apiUrl = environment.apiUrl + '/api/parkingspot';

	constructor(private http: HttpClient) { }

	getDashboard(): Observable<ParkingSpotDashboard[]> {
		return this.http.get<ParkingSpotDashboard[]>(`${this.apiUrl}/dashboard`);
	}

	getAll(): Observable<Spot[]> {
		return this.http.get<Spot[]>(this.apiUrl);
	}

	create(spot: { spotNumber: string }): Observable<Spot> {
		return this.http.post<Spot>(this.apiUrl, spot);
	}

	delete(id: number): Observable<any> {
		return this.http.delete<any>(`${this.apiUrl}/${id}`);
	}
}

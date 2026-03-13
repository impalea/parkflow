import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CheckIn {
  licensePlate: string;
  model: string;
	color: string;
	parkingSpotId: number;
}

export interface CheckOutPreview {
	ticketId: number;
	licensePlate: string;
	model: string;
	color: string;
	spotNumber: string;
	entryTime: string;
	exitTime: string;
	duration: string;
	totalAmount: string;
}

@Injectable({
  providedIn: 'root'
})
export class Ticket {
  private apiUrl = environment.apiUrl + '/api/ticket';

  constructor(private http: HttpClient) { }

  createCheckIn(data: CheckIn): Observable<any> {
    return this.http.post(`${this.apiUrl}/checkin`, data);
  }

	getCheckOutPreview(ticketId: number): Observable<CheckOutPreview> {
    return this.http.get<CheckOutPreview>(`${this.apiUrl}/checkout/preview/${ticketId}`);
  }

  confirmCheckOut(ticketId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/checkout/${ticketId}`, {});
  }
}

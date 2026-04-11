import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface PriceConfigs {
	id?: number;
	isActive: boolean;
	toleranceMinutes: number;
	firstHourValue: number;
	additionalHourValue: number;
	dailyValue: number;
	lastUpdatedAt?: string;
}

@Injectable({
	providedIn: 'root',
})
export class PriceConfigService {
	private apiUrl = environment.apiUrl + '/api/priceconfig';

	constructor(private http: HttpClient) { }

	getConfig(): Observable<PriceConfigs> {
		return this.http.get<PriceConfigs>(this.apiUrl);
	}

	updateConfig(config: PriceConfigs): Observable<PriceConfigs> {
		return this.http.put<PriceConfigs>(this.apiUrl, config);
	}
}

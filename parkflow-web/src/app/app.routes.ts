import { Routes } from '@angular/router';
import { Dashboard } from './components/dashboard/dashboard';
import { PriceConfig } from './components/price-config/price-config';
import { ParkingSpot } from './components/parking-spot/parking-spot';
import { Settings } from './components/settings/settings';

export const routes: Routes = [
	{ path: 'dashboard', component: Dashboard },
	{ path: 'settings', component: Settings },
	{ path: 'settings/prices', component: PriceConfig },
	{ path: 'settings/parking-spots', component: ParkingSpot },
	{ path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];

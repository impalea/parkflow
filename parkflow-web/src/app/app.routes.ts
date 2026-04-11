import { Routes } from '@angular/router';
import { Dashboard } from './components/dashboard/dashboard';
import { PriceConfig } from './components/price-config/price-config';

export const routes: Routes = [
	{ path: 'dashboard', component: Dashboard },
	{ path: 'settings', component: PriceConfig },
	{ path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];

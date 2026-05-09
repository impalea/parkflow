import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'app-settings',
	imports: [],
	templateUrl: './settings.html',
	styleUrl: './settings.scss',
})
export class Settings {
	constructor(private router: Router) {}

	goToHome() {
		this.router.navigate(['/']);
	}

	goTo(path: string) {
    this.router.navigate([path]);
  }
}

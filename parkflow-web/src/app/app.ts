import { Component, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
	selector: 'app-root',
	imports: [RouterOutlet],
	templateUrl: './app.html',
	styleUrl: './app.scss'
})
export class App {
	protected readonly title = signal('parkflow-web');

	constructor(private router: Router) {}

	goToSettings() {
		this.router.navigate(['/settings']);
	}

	goToHome() {
		this.router.navigate(['/']);
	}
}

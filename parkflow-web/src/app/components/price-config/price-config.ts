import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PriceConfigService } from '../../services/price-config/price-config';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-price-config',
	standalone: true,
	imports: [CommonModule, ReactiveFormsModule],
	templateUrl: './price-config.html',
	styleUrl: './price-config.scss',
})
export class PriceConfig implements OnInit {
	form: FormGroup;
	isLoading = signal(false);

	constructor(
		private fb: FormBuilder,
		private priceService: PriceConfigService,
		private router: Router
	) {
		this.form = this.fb.group({
			isActive: [false],
			toleranceMinutes: [0, [Validators.required, Validators.min(0)]],
			firstHourValue: [0, [Validators.required, Validators.min(0)]],
			additionalHourValue: [0, [Validators.required, Validators.min(0)]],
			dailyValue: [0, [Validators.required, Validators.min(0)]]
		});
	}

	ngOnInit(): void {
		this.loadSettings();

		this.form.get('isActive')?.valueChanges.subscribe((active: boolean) => {
			this.toggleInputs(active);
		});
	}

	loadSettings() {
		this.isLoading.set(true);

		this.priceService.getConfig().subscribe({
			next: (config) => {
				this.form.patchValue(config);
				this.toggleInputs(config.isActive);
				this.isLoading.set(false);
			},
			error: (err) => {
				console.error('Error during config loading:', err);
				this.isLoading.set(false);
			}
		});
	}

	saveSettings() {
		if (this.form.invalid) return;

		this.isLoading.set(true);
		const payload = this.form.value;

		this.priceService.updateConfig(payload).subscribe({
			next: () => {
				this.isLoading.set(false);
				this.form.markAsPristine();
			},
			error: (err) => {
				console.error('Error during config saving:', err);
				alert('Falha ao salvar configurações.');
				this.isLoading.set(false);
			}
		});
	}

	goBack() {
		this.router.navigate(['/']);
	}

	private toggleInputs(active: boolean) {
		const fields = ['toleranceMinutes', 'firstHourValue', 'additionalHourValue', 'dailyValue'];

		fields.forEach(field => {
			const control = this.form.get(field);
			if (active) {
				control?.enable();
			} else {
				control?.disable();
			}
		});
	}
}

import { Component, inject, Input, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgClass, NgIf } from '@angular/common';

@Component({
  selector: 'app-checkbox',
  standalone: true,
  imports: [
    FormsModule,
    NgClass,
    NgIf,
  ],
  templateUrl: './checkbox.component.html',
  styleUrl: './checkbox.component.css'
})
export class CheckboxComponent {
  @Input() id!: string;
  isChecked: WritableSignal<boolean> = signal(false);

  ngAfterViewInit() {
    setTimeout(() => {
      this.isChecked.update(old => old || sessionStorage.getItem(this.id) === 'true');
    }, 0);
  }

  toggleCheckbox() {
    this.isChecked.update(old => !old);
    sessionStorage.setItem(this.id, String(this.isChecked()));
  }
}

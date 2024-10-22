import { Component, signal } from '@angular/core';
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
  isChecked = signal(false);

  toggleCheckbox() {
    this.isChecked.update(old => !old);
  }
}

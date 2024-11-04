import { Component, inject, Input, ViewChild } from '@angular/core';
import { CheckboxComponent } from '../checkbox/checkbox.component';
import { ModalViewService } from '../modal-view.service';
import { NgClass, NgStyle } from '@angular/common';
import { Stop } from '../types';

@Component({
  selector: 'app-stop-card',
  standalone: true,
  imports: [
    CheckboxComponent,
    NgClass,
    NgStyle,
  ],
  templateUrl: './stop-card.component.html',
  styleUrl: './stop-card.component.css'
})
export class StopCardComponent {
  @Input() stop!: Stop;
  @Input() id!: string;
  @ViewChild (CheckboxComponent) checkbox!: CheckboxComponent;
  protected modalViewService = inject(ModalViewService);

  isChecked() {
    return this.checkbox?.isChecked() ?? false;
  }
}

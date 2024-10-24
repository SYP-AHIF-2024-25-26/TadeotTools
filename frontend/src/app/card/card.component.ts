import {
  ChangeDetectorRef,
  Component,
  computed,
  inject,
  Input,
  signal,
  ViewChild,
  WritableSignal,
} from '@angular/core';
import { Card } from '../main-page/main-page.component';
import { CheckboxComponent } from '../checkbox/checkbox.component';
import { CommonModule } from '@angular/common';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [
    CheckboxComponent,
    CommonModule,
  ],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  @Input() card!: Card;
  @Input() id!: string;
  @Input() bgColor!: string;
  @ViewChild (CheckboxComponent) checkbox!: CheckboxComponent;

  isChecked() {
    return this.checkbox?.isChecked() ?? false;
  }

}

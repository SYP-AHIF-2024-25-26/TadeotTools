import { Component, Input, signal } from '@angular/core';
import { Card } from '../main-page/main-page.component';
import { CheckboxComponent } from '../checkbox/checkbox.component';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [
    CheckboxComponent,
  ],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  @Input() card!: Card;

}

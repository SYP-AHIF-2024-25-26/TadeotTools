import { Component, computed, inject, signal, ViewChild } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeaderComponent } from '../header/header.component';
import { ViewModeService } from '../view-mode.service';

export type Card = {
  id: number;
  title: string;
  description: string;
}

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [
    CardComponent,
    NavbarComponent,
    HeaderComponent,
  ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  protected viewModeService = inject(ViewModeService);

  cards = signal<Card[]>([
    { id: 1, title: 'Card 1', description: 'Test 1' },
    { id: 2, title: 'Card 2', description: 'Test 2' },
  ]);

}

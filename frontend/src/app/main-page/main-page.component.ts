import { Component, signal } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeaderComponent } from '../header/header.component';

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
  cards = signal<Card[]>([
    { id: 1, title: 'Card 1', description: 'Test 1' },
    { id: 2, title: 'Card 2', description: 'Test 2' },
  ]);
}

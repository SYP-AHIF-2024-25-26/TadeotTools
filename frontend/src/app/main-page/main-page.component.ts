import { Component, inject, signal } from '@angular/core';
import { GuideCardComponent } from '../guide-card/guide-card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeaderComponent } from '../header/header.component';
import { ViewModeService } from '../view-mode.service';
import { GuideCard } from '../types';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [
    GuideCardComponent,
    NavbarComponent,
    HeaderComponent,
],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  protected viewModeService = inject(ViewModeService);


  cards = signal<GuideCard[]>([
    { id: 1, title: 'Kurzpräsentationen', description: 'Test 1' },
    { id: 2, title: 'Informatik Medientechnik - Tour', description: 'Test 2' },
    { id: 3, title: 'Elektronik Medizintechnik - Tour', description: 'Test 3' },
    { id: 4, title: 'Aula Highlights', description: 'Test 4' },
    { id: 5, title: 'Beratungszentrum', description: 'Test 5' },
    { id: 6, title: 'Voranmeldung', description: 'Test 6' },
  ]);


}


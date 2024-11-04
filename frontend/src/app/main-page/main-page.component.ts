import { Component, inject, signal } from '@angular/core';
import { GuideCardComponent } from '../guide-card/guide-card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeaderComponent } from '../header/header.component';
import { ViewModeService } from '../view-mode.service';
import { DescriptionModalComponent } from '../description-modal/description-modal.component';
import { ModalViewService } from '../modal-view.service';
import { GuideCard } from '../types';
import { StopGroupName } from '../stop-group-name.enum';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [
    GuideCardComponent,
    NavbarComponent,
    HeaderComponent,
    DescriptionModalComponent
],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  protected viewModeService = inject(ViewModeService);
  protected modalViewService = inject(ModalViewService);


  cards = signal<GuideCard[]>([
    { id: 1, title: 'Kurzpräsentationen', description: 'Test 1', isTour: false },
    { id: 2, title: 'Informatik Medientechnik - Tour', description: 'Test 2', isTour: true, stopGroupNames: [StopGroupName.Informatik, StopGroupName.Medientechnik, StopGroupName.Neutral] },
    { id: 3, title: 'Elektronik Medizintechnik - Tour', description: 'Test 3', isTour: true, stopGroupNames: [StopGroupName.Elektronik, StopGroupName.Medizintechnik, StopGroupName.Neutral] },
    { id: 4, title: 'Aula Highlights', description: 'Test 4', isTour: false },
    { id: 5, title: 'Beratungszentrum', description: 'Test 5', isTour: false },
    { id: 6, title: 'Voranmeldung', description: 'Test 6', isTour: false },
  ]);


}


import { Component, computed, inject } from '@angular/core';
import { HeaderComponent } from "../header/header.component";
import { NavbarComponent } from "../navbar/navbar.component";
import { BreadcrumbComponent } from "../breadcrumb/breadcrumb.component";
import { TourSelectionService } from '../tour-selection.service';
import { GuideCardComponent } from '../guide-card/guide-card.component';
import { ApiFetchService } from '../api-fetch.service';
import { Stop } from '../types';
import { StopCardComponent } from '../stop-card/stop-card.component';

@Component({
  selector: 'app-tour-page',
  standalone: true,
  imports: [HeaderComponent, NavbarComponent, BreadcrumbComponent, GuideCardComponent, StopCardComponent],
  templateUrl: './tour-page.component.html',
  styleUrl: './tour-page.component.css'
})
export class TourPageComponent {
  protected tourSelectionService = inject(TourSelectionService);
  protected apiFetchService = inject(ApiFetchService);

  tour: Stop[] = [];
  name = computed(() => this.tourSelectionService.getSelectedTour() as string);

  ngOnInit() {
    this.apiFetchService.getTour().then((tour) => {
      this.tour = tour;
    });
  }
}

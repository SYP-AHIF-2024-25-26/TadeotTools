import { Component, computed, inject, Input, Signal } from '@angular/core';
import { HeaderComponent } from "../header/header.component";
import { NavbarComponent } from "../navbar/navbar.component";
import { BreadcrumbComponent } from "../breadcrumb/breadcrumb.component";
import { GuideCardComponent } from '../guide-card/guide-card.component';
import { ApiFetchService } from '../api-fetch.service';
import { Stop } from '../types';
import { StopCardComponent } from '../stop-card/stop-card.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-stop-page',
  standalone: true,
  imports: [HeaderComponent, NavbarComponent, BreadcrumbComponent, StopCardComponent],
  templateUrl: './stop-page.component.html',
  styleUrl: './stop-page.component.css'
})
export class StopPageComponent {
  protected apiFetchService = inject(ApiFetchService);
  protected router = inject(Router);
  private route = inject(ActivatedRoute)
  id: Signal<string | null> = computed(() => {
    return this.route.snapshot.paramMap.get('id');
  })
  tour: Stop[] = [];


}

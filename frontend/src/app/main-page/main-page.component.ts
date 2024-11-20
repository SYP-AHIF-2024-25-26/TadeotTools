import { Component, computed, inject, signal, WritableSignal } from '@angular/core';
import { GuideCardComponent } from '../guide-card/guide-card.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { HeaderComponent } from '../header/header.component';
import { ViewModeService } from '../view-mode.service';
import { StopGroup } from '../types';
import { NgClass } from '@angular/common';
import { ApiFetchService } from '../api-fetch.service';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [
    GuideCardComponent,
    NavbarComponent,
    HeaderComponent,
    NgClass,
  ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {
  protected viewModeService = inject(ViewModeService);
  private apiFetchService = inject(ApiFetchService);
  groups: WritableSignal<StopGroup[]> = signal([])

  ngOnInit() {
    this.onLoad();
  }

  async onLoad() {
    this.groups.set(await this.apiFetchService.getStopGroups())
  }


}


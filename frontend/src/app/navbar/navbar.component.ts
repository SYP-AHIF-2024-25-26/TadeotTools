import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewModeService } from '../view-mode.service';
@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  protected viewModeService = inject(ViewModeService);

  switchToCardMode() {
    this.viewModeService.switchToCardMode();
  }
  switchToStationMode() {
    this.viewModeService.switchToStationMode();
  }
}

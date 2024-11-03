import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ViewModeService {

  constructor() { }

  private isStationMode = signal(true);

  switchToStationMode() {
    this.isStationMode.set(true);
  }

  switchToCardMode() {
    this.isStationMode.set(false);
  }

  viewMode() {
    return this.isStationMode();
  }
}

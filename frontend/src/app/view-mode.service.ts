import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ViewModeService {

  private isStationMode = signal(true);

  switchToStationMode() {
    this.isStationMode.set(true);
  }

  switchToCardMode() {
    this.isStationMode.set(false);
  }

  stationMode() {
    return this.isStationMode();
  }
}

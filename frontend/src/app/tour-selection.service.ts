import { Injectable, signal } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class TourSelectionService {
  private selectedTour = signal<string | null>(null);

  public setSelectedTour(tour: string) {
    this.selectedTour.set(tour);
  }

  public getSelectedTour() {
    return this.selectedTour();
  }

}

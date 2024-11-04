import { Injectable, signal } from '@angular/core';
import { StopGroupName } from './stop-group-name.enum';

@Injectable({
  providedIn: 'root'
})
export class ModalViewService {

  showModal = signal(false);
  modalTitle = "Sample Card Title";
  modalDescription = "This is a detailed description of the guide-card.";
  isTour = true;
  stopGroupNames: StopGroupName[] = [];

  openModal(title: string, description: string, isTour: boolean, stopGroupNames: StopGroupName[]) {
    this.modalTitle = title;
    this.modalDescription = description;
    this.isTour = isTour;
    if (stopGroupNames.length > 0) {
      this.stopGroupNames = stopGroupNames;
    }
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
  }

  modalState() {
    return this.showModal();
  }
}

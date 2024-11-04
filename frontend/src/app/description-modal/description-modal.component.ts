import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLinkActive } from '@angular/router';
import { TourSelectionService } from '../tour-selection.service';
import { ApiFetchService } from '../api-fetch.service';
import { StopGroupName } from '../stop-group-name.enum';

@Component({
  selector: 'app-description-modal',
  standalone: true,
  imports: [CommonModule, RouterLinkActive],
  templateUrl: './description-modal.component.html',
  styleUrl: './description-modal.component.css'
})
export class DescriptionModalComponent {
  @Input() title!: string;
  @Input() description!: string;
  @Input() isTour!: boolean;
  @Input() stopGroupNames!: StopGroupName[];
  @Output() closeModal = new EventEmitter<void>();

  protected router = inject(Router);
  protected tourSelectionService = inject(TourSelectionService);
  protected apiFetchService = inject(ApiFetchService);


  async onTourJoin() {
    this.tourSelectionService.setSelectedTour(this.title);
    this.apiFetchService.setTourGroups(this.stopGroupNames);
    this.closeModal.emit();
    await this.router.navigate(['/tour']);
  }

  onClose() {
    this.closeModal.emit();
  }
}

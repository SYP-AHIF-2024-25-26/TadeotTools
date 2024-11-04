import {
  Component, computed,
  inject,
  Input, signal,
  ViewChild,
} from '@angular/core';
import { CheckboxComponent } from '../checkbox/checkbox.component';
import { CommonModule } from '@angular/common';
import { ModalViewService } from '../modal-view.service';
import { GuideCard } from '../types';
import { StopGroupName } from '../stop-group-name.enum';
import { ApiFetchService } from '../api-fetch.service';
@Component({
  selector: 'app-guide-card',
  standalone: true,
  imports: [
    CheckboxComponent,
    CommonModule,
  ],
  templateUrl: './guide-card.component.html',
  styleUrl: './guide-card.component.css'
})
export class GuideCardComponent {
  @Input() card!: GuideCard;
  @Input() id!: string;
  @Input() stopGroupNames!: StopGroupName[];
  @ViewChild (CheckboxComponent) checkbox!: CheckboxComponent;
  protected modalViewService = inject(ModalViewService);
  protected apiFetchService = inject(ApiFetchService);

  progress = computed(() => {
    const IDs = sessionStorage.getItem(this.stopGroupNames.join(','));
    if (IDs === null) {
      return null;
    }
    const stopIDs = JSON.parse(IDs) as string[];
    let completed = 0;
    stopIDs.forEach((id) => {
      if (sessionStorage.getItem(id) === 'true') {
        completed++;
      }
    });
    return completed;
  });
  max = computed(() => {
    const IDs = sessionStorage.getItem(this.stopGroupNames.join(','));
    if (IDs === null) {
      return null;
    }
    return JSON.parse(IDs).length;
  });
  isChecked() {
    return this.checkbox?.isChecked() ?? false;
  }


  protected readonly String = String;
}

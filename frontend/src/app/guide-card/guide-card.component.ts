import {
  Component, inject,
  Input, ViewChild,
} from '@angular/core';
import { CheckboxComponent } from '../checkbox/checkbox.component';
import { CommonModule } from '@angular/common';
import { StopGroup } from '../types';
import { Router } from '@angular/router';

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
  @Input() stopGroup!: StopGroup;
  @Input() id!: string;
  @ViewChild(CheckboxComponent) checkbox!: CheckboxComponent;

  protected router = inject(Router);

  //progress = computed(() => this.calculateProgress());
  // max = computed(() => this.calculateMax());

  /*private calculateProgress(): number | null {
    const IDs = sessionStorage.getItem(this.stopGroupNames.join(','));
    if (IDs === null) {
      return null;
    }
    const stopIDs = JSON.parse(IDs) as string[];
    return stopIDs.filter(id => sessionStorage.getItem(id) === 'true').length;
  }

  private calculateMax(): number | null {
    const IDs = sessionStorage.getItem(this.stopGroupNames.join(','));
    if (IDs === null) {
      return null;
    }
    return JSON.parse(IDs).length;
  }
  */

  isChecked(): boolean {
    return this.checkbox?.isChecked() ?? false;
  }

  openStopPage() {
    this.router.navigate([`tour/${this.stopGroup.stopGroupID}`], )
  }
}

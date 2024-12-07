import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Info} from "../types";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-stopgroups-info-popup',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './stopgroups-info-popup.component.html',
  styleUrl: './stopgroups-info-popup.component.css'
})
export class StopgroupsInfoPopupComponent {
  @Input() info: Info | undefined;
  @Output() deleted = new EventEmitter<number>();

  closePopup() {
    this.deleted.emit(this.info!.id);
  }
}

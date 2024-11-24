import {Component, inject, signal} from '@angular/core';
import {Stop, StopGroup} from "../types";
import {StopgroupService} from "../stopgroup.service";
import {StopService} from "../stop.service";
import {CdkDragDrop, moveItemInArray, transferArrayItem, CdkDropList, CdkDrag} from "@angular/cdk/drag-drop";

@Component({
  selector: 'app-stopgroups',
  standalone: true,
  imports: [CdkDropList, CdkDrag],
  templateUrl: './stopgroups.component.html',
  styleUrl: './stopgroups.component.css'
})
export class StopgroupsComponent {
  stopGroupFetcher = inject(StopgroupService);
  stopFetcher = inject(StopService);

  stopGroups = signal<StopGroup[]>([]);
  privateStops = signal<Stop[]>([]);

  constructor() {
    this.getStopGroups();
    this.initialisePrivateStops();
  }

  async getStopGroups() {
    this.stopGroups.set(await this.stopGroupFetcher.getStopGroups());
  }

  dropGroup(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.stopGroups(), event.previousIndex, event.currentIndex);
    console.log('dropped');
  }

  dropStop(event: CdkDragDrop<any[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
          event.previousContainer.data,
          event.container.data,
          event.previousIndex,
          event.currentIndex,
      );
    }
  }

  getDropGroups(): string[] {
    return this.stopGroups().map(group => 'group-' + group.stopGroupID);
  }

  async initialisePrivateStops() {
    this.privateStops.set(await this.stopFetcher.getPrivateStops());
    console.log('private stops', this.privateStops());
  }

  updateOrder() {

  }
}

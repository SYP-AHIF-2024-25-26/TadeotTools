import {Component, inject, signal} from '@angular/core';
import {Stop, StopGroup} from "../types";
import {StopGroupService} from "../stopgroup.service";
import {StopService} from "../stop.service";
import {CdkDragDrop, moveItemInArray, transferArrayItem, CdkDropList, CdkDrag} from "@angular/cdk/drag-drop";
import {group} from "@angular/animations";
import {window} from "rxjs";

@Component({
    selector: 'app-stopgroups',
    standalone: true,
    imports: [CdkDropList, CdkDrag],
    templateUrl: './stopgroups.component.html',
    styleUrl: './stopgroups.component.css'
})
export class StopgroupsComponent {
    stopGroupFetcher = inject(StopGroupService);
    stopFetcher = inject(StopService);


    stopGroups = signal<StopGroup[]>([]);
    allStops = signal<Stop[]>([]);

    constructor() {
        this.initialiseStops();
        this.getStopGroups();
    }

    getStopsByGroupID(groupID: number | null): Stop[] {
        return this.allStops().filter(stop => stop.stopGroupID === groupID)
    }

    async getStopGroups() {
        this.stopGroups.set(await this.stopGroupFetcher.getStopGroups());
    }

    dropGroup(event: CdkDragDrop<any[]>) {
        moveItemInArray(this.stopGroups(), event.previousIndex, event.currentIndex);
    }

    dropStop(event: CdkDragDrop<any[]>) {
      const groupId = this.getGroupIdFromEvent(event);
      if (groupId === null) {
        this.allStops()[event.currentIndex].stopGroupID = null;
      } else {
        console.log('group-' + groupId);
        const maxIndex = this.allStops().filter(stop => stop.stopGroupID !== null && stop.stopGroupID <= groupId).length;
        const index = maxIndex - event.currentIndex;
        console.log(index);
      }
      /*const previousGroupId = event.previousContainer.id.split('-')[1];
      const oldId = this.allStops().filter(stop => stop.stopGroupID === (previousGroupId === null ? null : parseInt(previousGroupId))).map(stop => stop.stopID)[0] + event.previousIndex;
      const groupId = event.container.id.split('-')[1];
      const idkWhatThisIs = this.allStops().filter(stop => stop.stopGroupID === (groupId === null ? null : parseInt(groupId))).map(stop => stop.stopID)[0];
      const newId =  + event.currentIndex;
      this.allStops()[event.currentIndex].stopGroupID = groupId === '0' ? null : parseInt(groupId);
      console.log(oldId, newId);
      moveItemInArray(this.allStops(), event.previousIndex, event.currentIndex);*/
    }

    getGroupIdFromEvent(event: CdkDragDrop<any[]>): number | null {
      const groupIdString = event.container.id.split('-')[1];
      return groupIdString === null ? null : parseInt(groupIdString);
    }

    getDropGroups(): string[] {
        return [...this.stopGroups().map(group => 'group-' + group.stopGroupID), 'group-0'];
    }

    async initialiseStops() {
        /*const stops = await this.stopFetcher.getAllStops();
        if (stops) {
            this.allStops.set(stops);
        }*/
    }

    updateOrder() {
        this.stopGroupFetcher.updateStopGroupOrder(this.stopGroups().map(group => group.stopGroupID));
        console.log(this.allStops().map(stop => stop.stopID));
        this.stopFetcher.updateStopOrder(this.allStops().map(stop => stop.stopID));
    }
}

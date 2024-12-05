import {Component, inject, OnInit, signal} from '@angular/core';
import {Division, Stop, StopGroupWithStops} from "../types";
import {StopGroupService} from "../stopgroup.service";
import {StopService} from "../stop.service";
import {CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import {RouterLink} from "@angular/router";
import {DivisionService} from "../division.service";
import {NgClass, NgStyle} from "@angular/common";

@Component({
  selector: 'app-stopgroups',
  standalone: true,
  imports: [CdkDropList, CdkDrag, RouterLink, NgClass, NgStyle],
  templateUrl: './stopgroups.component.html',
  styleUrl: './stopgroups.component.css'
})
export class StopGroupsComponent implements OnInit {
  stopGroupFetcher = inject(StopGroupService);
  stopFetcher = inject(StopService);
  divisionFetcher = inject(DivisionService);
  hasChanged = signal<boolean>(false);

  stopGroups = signal<StopGroupWithStops[]>([]);
  privateStops = signal<Stop[]>([]);
  stopsToUpdate = signal<Stop[]>([]);
  divisions = signal<Division[]>([]);

  async ngOnInit() {
    await this.initialiseData();
  }

  async initialiseData() {
    await this.getDivisions();
    await this.getStopGroups();
    await this.getPrivateStops();
    this.hasChanged.set(false);
  }

  // Fetching Functions for StopGroups and PrivateStops
  async getStopGroups() {
    this.stopGroups.set(await this.stopGroupFetcher.getStopGroups());
  }

  async getPrivateStops() {
    const stops = await this.stopFetcher.getPrivateStops();
    this.privateStops.set(stops);
  }

  async getDivisions() {
    const divisions = await this.divisionFetcher.getDivisions();
    console.log(divisions);
    this.divisions.set(divisions);
  }

  // Drag and Drop Function for Groups
  dropGroup(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.stopGroups(), event.previousIndex, event.currentIndex);
    this.hasChanged.set(true);
  }

  // Drag and Drop Function for Stops
  dropStop(event: CdkDragDrop<any[]>) {
    const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
    const currentGroupId = parseInt(event.container.id.split('-')[1]);

    this.hasChanged.set(true);
    if (previousGroupId === 0 && currentGroupId === 0) {
      this.dropStopFromUnassignedToUnassigned(event);
    } else if (currentGroupId === 0) {
      this.dropStopFromAssignedToUnassigned(event);
    } else if (previousGroupId !== 0) {
      this.dropStopFromAssignedToAssigned(event);
    } else {
      this.dropStopFromUnassignedToAssigned(event);
    }
    // console.log(this.stopGroups());
    // console.log(this.privateStops());
  }

  dropStopFromUnassignedToUnassigned(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.privateStops(), event.previousIndex, event.currentIndex);
  }

  dropStopFromUnassignedToAssigned(event: CdkDragDrop<any[]>) {
    const stopGroupId = parseInt(event.container.id.split('-')[1]);

    const stop = this.privateStops()[event.previousIndex];
    stop.stopGroupID = stopGroupId;
    this.stopsToUpdate.update(stops => [...stops, stop]);
    transferArrayItem(this.privateStops(), this.stopGroups().find(group => group.stopGroupID === stopGroupId)!.stops, event.previousIndex, event.currentIndex);
  }

  dropStopFromAssignedToUnassigned(event: CdkDragDrop<any[]>) {
    const stopGroupId = parseInt(event.previousContainer.id.split('-')[1]);
    transferArrayItem(this.stopGroups().find(group => group.stopGroupID === stopGroupId)!.stops, this.privateStops(), event.previousIndex, event.currentIndex);
    const stop = this.privateStops()[event.currentIndex];
    stop.stopGroupID = null;
    this.stopsToUpdate.update(stops => [...stops, stop]);
  }

  dropStopFromAssignedToAssigned(event: CdkDragDrop<any[]>) {
    const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
    const currentGroupId = parseInt(event.container.id.split('-')[1]);

    const previousStops = this.stopGroups().find(group => group.stopGroupID === previousGroupId)!.stops
    const currentStops = this.stopGroups().find(group => group.stopGroupID === currentGroupId)!.stops
    transferArrayItem(previousStops, currentStops, event.previousIndex, event.currentIndex);
    const stop = currentStops[event.currentIndex];
    stop.stopGroupID = currentGroupId;
    this.stopsToUpdate.update(stops => [...stops, stop]);
  }

  updateOrder() {
    // Update the order of the stops
    const stopOrder = [this.stopGroups().map(group => group.stops).flat().map(stop => stop.stopID), this.privateStops().map(stop => stop.stopID)].flat();
    this.stopFetcher.updateStopOrder(stopOrder);

    // Update the stopGroupID of the stops
    this.stopsToUpdate().forEach(stop => {
      this.stopFetcher.updateStopStopGroupId(stop);
      console.log("Updating stop with ID: " + stop.stopID + " to stopGroupID: " + stop.stopGroupID);
    });
    this.stopsToUpdate.set([]);

    // Update the order of the stopGroups
    const stopGroupOrder = this.stopGroups().map(group => group.stopGroupID);
    this.stopGroupFetcher.updateStopGroupOrder(stopGroupOrder);

    this.hasChanged.set(false);
  }

  getGroupIdFromEvent(event: CdkDragDrop<any[]>): number | null {
    const groupIdString = event.container.id.split('-')[1];
    return groupIdString === null ? null : parseInt(groupIdString);
  }

  getDropGroups(): string[] {
    return [...this.stopGroups().map(group => 'group-' + group.stopGroupID), 'group-0'];
  }

  getColorCodeByStopId(divisionId: number): string {
    console.log(this.divisions());
    return this.divisions().find(division => division.divisionID === divisionId)!.color;
  }
}

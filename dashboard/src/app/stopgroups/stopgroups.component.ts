import {Component, inject, signal} from '@angular/core';
import {Stop, StopGroup} from "../types";
import {StopgroupService} from "../stopgroup.service";
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
    stopGroupFetcher = inject(StopgroupService);
    stopFetcher = inject(StopService);

    stopGroups = signal<StopGroup[]>([]);
    privateStops = signal<Stop[]>([]);

    constructor() {
        this.initialisePrivateStops();
        this.getStopGroups();
    }

    async getStopGroups() {
        this.stopGroups.set(await this.stopGroupFetcher.getStopGroups());
    }

    dropGroup(event: CdkDragDrop<any[]>) {
        moveItemInArray(this.stopGroups(), event.previousIndex, event.currentIndex);
    }

    dropStop(event: CdkDragDrop<any[]>) {
        if (event.previousContainer === event.container) {
            moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
        } else if (event.previousContainer.id === 'group-0') {

            transferArrayItem(
                this.privateStops(),
                event.container.data,
                event.previousIndex,
                event.currentIndex,
            );
        } else {
            transferArrayItem(
                event.previousContainer.data,
                event.container.id === 'group-0' ? this.privateStops() : event.container.data,
                event.previousIndex,
                event.currentIndex,
            );
        }
    }

    getDropGroups(): string[] {
        return [...this.stopGroups().map(group => 'group-' + group.stopGroupID), 'group-0'];
    }

    async initialisePrivateStops() {
        const stops = await this.stopFetcher.getPrivateStops();
        if (stops) {
            this.privateStops.set(stops);
        }
    }

    updateOrder() {
        this.stopGroupFetcher.updateStopGroupOrder(this.stopGroups().map(group => group.stopGroupID));
        this.stopFetcher.updateStopOrder(
            [
                this.stopGroups().map(group => group.stops.map(stop => stop.stopID)).flat(),
                this.privateStops().map(stop => stop.stopID)
            ].flat());
    }
}

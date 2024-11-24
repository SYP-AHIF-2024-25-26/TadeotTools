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
        moveItemInArray(this.allStops(), event.previousIndex, event.currentIndex);
    }

    getDropGroups(): string[] {
        return [...this.stopGroups().map(group => 'group-' + group.stopGroupID), 'group-0'];
    }

    async initialiseStops() {
        const stops = await this.stopFetcher.getAllStops();
        if (stops) {
            this.allStops.set(stops);
        }
    }

    updateOrder() {
        this.stopGroupFetcher.updateStopGroupOrder(this.stopGroups().map(group => group.stopGroupID));
        this.stopFetcher.updateStopOrder(this.allStops().map(stop => stop.stopID));
    }
}

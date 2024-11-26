import {Component, inject, signal} from '@angular/core';
import {CdkDrag, CdkDragDrop, CdkDropList, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import {StopGroupService} from "../stopgroup.service";
import {StopService} from "../stop.service";
import {Stop, StopGroup, StopGroupWithStops} from "../types";
import {group} from "@angular/animations";

@Component({
    selector: 'app-test',
    standalone: true,
    imports: [
        CdkDrag,
        CdkDropList
    ],
    templateUrl: './test.component.html',
    styleUrl: './test.component.css'
})
export class TestComponent {
    stopGroupFetcher = inject(StopGroupService);
    stopFetcher = inject(StopService);

    stopGroups = signal<StopGroupWithStops[]>([]);
    privateStops = signal<Stop[]>([]);
    stopsToUpdate = signal<Stop[]>([]);

    constructor() {
        this.getStopGroups();
        console.log('TEst');
        this.getPrivateStops();
    }

    async getStopGroups() {
        this.stopGroups.set(await this.stopGroupFetcher.getStopGroups());
    }

    async getPrivateStops() {
        const stops = await this.stopFetcher.getPrivateStops();
        console.log(stops);
        this.privateStops.set(stops);
    }

    dropGroup(event: CdkDragDrop<any[]>) {
        moveItemInArray(this.stopGroups(), event.previousIndex, event.currentIndex);
        console.log(this.stopGroups())
    }

    dropToUnassigned(event: CdkDragDrop<any[]>) {
        const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
        if (previousGroupId === 0) {
            moveItemInArray(
                this.privateStops(),
                event.previousIndex,
                event.currentIndex
            );
        } else {
            transferArrayItem(
                this.stopGroups()
                    .find(group => group.stopGroupID === previousGroupId)!.stops,
                this.privateStops(),
                event.previousIndex,
                event.currentIndex
            );
            const stop = this.privateStops()[event.currentIndex];
            stop.stopGroupID = null;
            this.stopsToUpdate.update(stops => [...stops, this.privateStops()[event.currentIndex]]);
            console.log(this.stopGroups());
        }
    }

    dropStop(event: CdkDragDrop<any[]>) {
        const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
        const currentGroupId = parseInt(event.container.id.split('-')[1]);

        if (previousGroupId !== 0) {
            this.dropFromGroup(event);
        } else {
            this.dropFromNoGroup(event);
        }
    }

    getGroupIdFromEvent(event: CdkDragDrop<any[]>): number | null {
        const groupIdString = event.container.id.split('-')[1];
        return groupIdString === null ? null : parseInt(groupIdString);
    }

    getDropGroups(): string[] {
        return [...this.stopGroups().map(group => 'group-' + group.stopGroupID), 'group-0'];
    }

    updateOrder() {
        this.stopGroupFetcher.updateStopGroupOrder(this.stopGroups().map(group => group.stopGroupID));
        console.log([this.stopGroups().map(stop => stop.stops).map(stops => stops.map(stop => stop.stopID)).flat(), this.privateStops().map(stop => stop.stopID)].flat());
        this.stopFetcher.updateStopOrder([this.stopGroups().map(stop => stop.stops).map(stops => stops.map(stop => stop.stopID)).flat(), this.privateStops().map(stop => stop.stopID)].flat());
        this.stopsToUpdate().forEach(stop => {
            this.stopFetcher.updateStopStopGroupId(stop);
        });
        this.stopsToUpdate.set([]);
    }

    dropFromNoGroup(event: CdkDragDrop<any>) {
        const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
        const currentGroupId = parseInt(event.container.id.split('-')[1]);

        const stop = this.privateStops()[event.previousIndex]
        stop.stopGroupID = currentGroupId;
        this.stopsToUpdate.update(stops => [...stops, stop]);

        transferArrayItem(
            this.privateStops(),
            this.stopGroups()
                .find(group => group.stopGroupID === currentGroupId)!.stops,
            event.previousIndex,
            event.currentIndex
        );


        console.log(this.stopGroups())
    }

    dropFromGroup(event: CdkDragDrop<any[]>) {
        const previousGroupId = parseInt(event.previousContainer.id.split('-')[1]);
        const currentGroupId = parseInt(event.container.id.split('-')[1]);

        transferArrayItem(
            this.stopGroups()
                .find(group => group.stopGroupID === previousGroupId)!.stops,
            this.stopGroups()
                .find(group => group.stopGroupID === currentGroupId)!.stops,
            event.previousIndex,
            event.currentIndex
        );
        const stop = this.stopGroups().find(group => group.stopGroupID === currentGroupId)!.stops[event.currentIndex];
        stop.stopGroupID = currentGroupId;
        this.stopsToUpdate.update(stops => [...stops, stop]);

        console.log(this.stopGroups());
    }
}

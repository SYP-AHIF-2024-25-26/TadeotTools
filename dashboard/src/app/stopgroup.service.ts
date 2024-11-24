import {inject, Injectable} from '@angular/core';
import {ResponseStopGroup, StopGroup} from "./types";
import {StopService} from "./stop.service";

@Injectable({
    providedIn: 'root'
})
export class StopgroupService {
    stopService = inject(StopService);

    constructor() {
    }

    async getStopGroups(): Promise<StopGroup[]> {
        const response = await fetch(`http://localhost:5000/v1/groups`);
        const responseStopGroups = await response.json() as ResponseStopGroup[];
        return Promise.all(
            responseStopGroups.map(async stops => ({
                ...stops,
                stops: await this.stopService.getStopsByStopGroupID(stops.stopGroupID)
            } as StopGroup))
        );
    }
}

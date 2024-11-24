import {inject, Injectable} from '@angular/core';
import {Stop} from "./types";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../environments/environment.development";

@Injectable({
    providedIn: 'root'
})
export class StopService {
    httpClient = inject(HttpClient);

    public async getPrivateStops() {
        return firstValueFrom(this.httpClient.get<Stop[]>(environment.API_URL + 'api/stops/private', {
            headers: {
                'x-api-key': environment.API_KEY,
            }
        }));
    }

    constructor() {
    }

    async getStopsByStopGroupID(stopGroupID: number): Promise<Stop[]> {
        return firstValueFrom(this.httpClient.get<Stop[]>(environment.API_URL + `stops/groups/${stopGroupID}`));
    }


    updateStopOrder(stops: number[]) {
        this.httpClient.put(environment.API_URL + 'api/stops/order', {
                'order': stops
            },
            {
                headers: {
                    "X-Api-Key": environment.API_KEY,
                }
            });
    }
}

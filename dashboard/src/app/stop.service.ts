import {inject, Injectable} from '@angular/core';
import {Stop} from "./types";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {BASE_URL} from "./app.config";

@Injectable({
    providedIn: 'root'
})
export class StopService {
    httpClient = inject(HttpClient);
    baseUrl = inject(BASE_URL);

    public async getAllStops() {
        return firstValueFrom(this.httpClient.get<Stop[]>(this.baseUrl + '/api/stops', {
            headers: {
                'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
            }
        }));
    }

    constructor() {
    }

    async getStopsByStopGroupID(stopGroupID: number): Promise<Stop[]> {
        return firstValueFrom(this.httpClient.get<Stop[]>(this.baseUrl + `/stops/groups/${stopGroupID}`));
    }


    updateStopOrder(stops: number[]) {
        this.httpClient.put(this.baseUrl + '/api/stops/order', {
                'order': stops
            },
            {
                headers: {
                    'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                }
            });
    }
}

import {inject, Injectable} from '@angular/core';
import {StopGroup} from "./types";
import {StopService} from "./stop.service";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom, map} from "rxjs";
import {BASE_URL} from "./app.config";

@Injectable({
    providedIn: 'root'
})
export class StopgroupService {
    stopService = inject(StopService);
    httpClient = inject(HttpClient);
    baseUrl = inject(BASE_URL);

    async getStopGroups(): Promise<StopGroup[]> {
        return firstValueFrom(this.httpClient.get<StopGroup[]>(this.baseUrl + '/groups'));
    }

    updateStopGroupOrder(stopGroups: number[]) {
        firstValueFrom(this.httpClient.put(this.baseUrl + `/api/groups/order`, {
            'order': stopGroups
        }, {
            headers: {
                "X-Api-Key": localStorage.getItem('API_KEY') ?? '',
            }
        }));
    }
}


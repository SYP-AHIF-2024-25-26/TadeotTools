import {inject, Injectable} from '@angular/core';
import {Stop} from './types';
import {HttpClient} from '@angular/common/http';
import {firstValueFrom} from 'rxjs';
import {BASE_URL} from './app.config';

@Injectable({
    providedIn: 'root',
})
export class StopService {
    httpClient = inject(HttpClient);
    baseUrl = inject(BASE_URL);

    constructor() {
    }

    public async getPrivateStops() {
        return firstValueFrom(
            this.httpClient.get<Stop[]>(this.baseUrl + '/api/stops/private', {
                headers: {
                    'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                },
            })
        );
    }

    async getStopsByStopGroupID(stopGroupID: number): Promise<Stop[]> {
        return firstValueFrom(
            this.httpClient.get<Stop[]>(this.baseUrl + `/stops/groups/${stopGroupID}`)
        );
    }

    updateStopOrder(stops: number[]) {
        firstValueFrom(
            this.httpClient.put(
                this.baseUrl + '/api/stops/order', { order: stops }, {
                    headers: {
                        'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                    },
                }
            )
        );
    }

    async updateStopStopGroupId(stop: Stop) {
        return await firstValueFrom(
            this.httpClient.put(
                this.baseUrl + `/api/stops/` + stop.stopID,
                {
                    name: stop.name,
                    description: stop.description,
                    roomNr: stop.roomNr,
                    stopGroupId: stop.stopGroupID,
                    divisionId: stop.divisionID,
                },
                {
                    headers: {
                        'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                    },
                }
            )
        );
    }

    async addStop(stop: {
        name: string;
        description: string;
        roomNr: string;
        divisionID: number;
    }) {
        await firstValueFrom(
            this.httpClient.post(this.baseUrl + '/api/stops', stop, {
                headers: {
                    'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                },
            })
        );
    }

    async updateStop(stop: Stop) {
        await firstValueFrom(
            this.httpClient.put(this.baseUrl + `/api/stops/${stop.stopID}`, stop, {
                headers: {
                    'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                },
            })
        );
    }

    async deleteStop(stopId: number) {
        await firstValueFrom(
            this.httpClient.delete(this.baseUrl + `/api/stops/${stopId}`, {
                headers: {
                    'X-Api-Key': localStorage.getItem('API_KEY') ?? '',
                },
            })
        );
    }
}

import {inject, Injectable} from '@angular/core';
import {ResponseStopGroup, StopGroup} from "./types";
import {StopService} from "./stop.service";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom, map} from "rxjs";
import {environment} from "../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class StopgroupService {
  stopService = inject(StopService);
  httpClient = inject(HttpClient);

  constructor() {
  }

  async getStopGroups(): Promise<StopGroup[]> {
    const response = await firstValueFrom(this.httpClient.get<ResponseStopGroup[]>(environment.API_URL + 'groups'));
    return Promise.all(
      response.map(async stops => ({
        ...stops,
        stops: await this.stopService.getStopsByStopGroupID(stops.stopGroupID)
      } as StopGroup))
    );
  }

  async updateStopGroupOrder(stopGroups: number[]) {
    const response = await firstValueFrom(this.httpClient.put(environment.API_URL + `api/groups/order`, {
      'order': stopGroups
    }, {
      headers: {
        "X-Api-Key": environment.API_KEY,
      }
    }));
  }
}


import { Injectable } from '@angular/core';
import {Stop} from "./types";

@Injectable({
  providedIn: 'root'
})
export class StopService {
  public async getPrivateStops() {
    const response = await fetch('http://localhost:5000/v1/stops/private');
    return await response.json() as Stop[];
  }
  constructor() { }

  async getStopsByStopGroupID(stopGroupID: number) {
    const response = await fetch(`http://localhost:5000/v1/stops/groups/${stopGroupID}`);
    return await response.json() as Stop[];
  }
}

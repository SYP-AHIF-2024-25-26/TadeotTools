import { inject, Injectable } from '@angular/core';
import { Stop, StopGroup } from './types';
import { StopGroupName } from './stop-group-name.enum';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ApiFetchService {
  tour: StopGroupName[] = [];
  http = inject(HttpClient);

  private async getStopGroupIDs(): Promise<string[]> {
    let result: string[] = [];
    let response = (await (await fetch(`http://localhost:5000/v1/groups`)).json()) as StopGroup[];
    response.forEach((group) => {
      if (this.tour.includes(group.name as StopGroupName)) {
        result.push(group.stopGroupID.toString());
      }
    });
    return result;
  }

  setTourGroups(stopGroups: StopGroupName[]) {
    this.tour = stopGroups;
  }

  public async getTour(): Promise<Stop[]> {
    if (this.tour.length === 0) {
      return [];
    }
    let result: Stop[] = [];

    const groupIDs = await this.getStopGroupIDs();

    for (let groupID of groupIDs) {
      let response = (await (await fetch(`http://localhost:5000/v1/stops/groups/${groupID}`)).json()) as Stop[];
      result = result.concat(response);
    }

    sessionStorage.setItem(this.tour.join(','), JSON.stringify(result.map((stop) => 'stop-card' + stop.stopID)));

    result.forEach((stop) => {
      if (sessionStorage.getItem('stop-card' + stop.stopID) === null) {
        sessionStorage.setItem('stop-card' + stop.stopID, 'false');
      } else {
        sessionStorage.setItem('stop-card' + stop.stopID, <string>sessionStorage.getItem('stop-card' + stop.stopID));
      }
    });

    return result;
  }
}

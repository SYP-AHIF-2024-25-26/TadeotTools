import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import {BASE_URL} from './app.config';
import { firstValueFrom } from 'rxjs';

export type Division = {
  divisionID: number,
  name: string,
  color: string,
  image: null | string
}

@Injectable({
  providedIn: 'root'
})
export class DivisionService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseUrl = inject(BASE_URL);

  constructor() { }

  async getDivisions(): Promise<Division[]> {
    return firstValueFrom(this.httpClient.get<Division[]>(`${this.baseUrl}/divisions`));
  }

  async deleteDivision(divisionId: number): Promise<void> {
    return;
  }
}

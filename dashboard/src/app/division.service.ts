import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BASE_URL } from './app.config';
import { firstValueFrom } from 'rxjs';
import { Division } from './types';

@Injectable({
  providedIn: 'root',
})
export class DivisionService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseUrl = inject(BASE_URL);

  constructor() {}

  async getDivisions(): Promise<Division[]> {
    return firstValueFrom(
      this.httpClient.get<Division[]>(`${this.baseUrl}/divisions`)
    );
  }

  async addDivision(division: {
    name: string;
    color: string;
    image: number[] | null;
  }): Promise<void> {
    await firstValueFrom(this.httpClient.post(`${this.baseUrl}/api/divisions`, division, {
      headers: {
        'X-Api-Key': localStorage.getItem('API_KEY') || '',
      },
    }));
  }

  async updateDivision(division: Division): Promise<void> {
    console.log(division.image);
    await firstValueFrom(this.httpClient.put(
      `${this.baseUrl}/api/divisions/${division.divisionID}`,
      division,
      {
        headers: {
          'X-Api-Key': localStorage.getItem('API_KEY') || '',
        },
      }
    ));
  }

  async deleteDivision(divisionId: number): Promise<void> {
    await firstValueFrom(this.httpClient.delete(`${this.baseUrl}/api/divisions/${divisionId}`, {
      headers: {
        'X-Api-Key': localStorage.getItem('API_KEY') || '',
      },
    }));
  }
}

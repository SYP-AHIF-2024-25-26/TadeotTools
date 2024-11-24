import { ApplicationConfig, InjectionToken, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { routes } from './app.routes';
export const BASE_URL = new InjectionToken<string>('BaseUrl');

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    { provide: BASE_URL, useValue: 'http://localhost:5000/v1' },
    provideRouter(routes),
    provideHttpClient(withFetch()),
  ]
};

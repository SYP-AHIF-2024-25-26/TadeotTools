import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { StopDescriptionPageComponent } from './stop-description-page/stop-description-page.component';
import { MapComponent } from './map/map.component';
import { StopPageComponent } from './stop-page/stop-page.component';

export const routes: Routes = [
  { path: 'main', component: MainPageComponent },
  { path: 'tour/:stopGroupId', component: StopPageComponent },
  { path: 'tour/:stopGroupId/stop/:stopId', component: StopDescriptionPageComponent },
  { path: 'map', component: MapComponent },
  { path: 'map/:roomNr', component: MapComponent },
  { path: '', redirectTo: '/main', pathMatch: 'full' },
];

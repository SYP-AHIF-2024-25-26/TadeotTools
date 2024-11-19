import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { StopPageComponent } from './stop-page/stop-page.component';

export const routes: Routes = [
  { path: 'main', component: MainPageComponent },
  { path: 'tour/:id', component: StopPageComponent },
  { path: '', redirectTo: '/main', pathMatch: 'full' },
];

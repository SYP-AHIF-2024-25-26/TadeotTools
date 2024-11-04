import { Routes } from '@angular/router';
import { MainPageComponent } from './main-page/main-page.component';
import { TourPageComponent } from './tour-page/tour-page.component';

export const routes: Routes = [
  { path: 'main', component: MainPageComponent },
  { path: 'tour', component: TourPageComponent },
  { path: '', redirectTo: '/main', pathMatch: 'full' },
];

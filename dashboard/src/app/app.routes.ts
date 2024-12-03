import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { StopgroupsComponent } from './stopgroups/stopgroups.component';
import { DivisionsListComponent } from './divisions-list/divisions-list.component';
import { DivisionDetailsComponent } from './division-details/division-details.component';
import { StopDetailsComponent } from './stop-details/stop-details.component';
import { StopgroupDetailsComponent } from './stopgroup-details/stopgroup-details.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'stopgroups', component: StopgroupsComponent },
  { path: 'divisions', component: DivisionsListComponent },
  { path: 'division', component: DivisionDetailsComponent },
  { path: 'stop', component: StopDetailsComponent },
  { path: 'stopgroup', component: StopgroupDetailsComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
];

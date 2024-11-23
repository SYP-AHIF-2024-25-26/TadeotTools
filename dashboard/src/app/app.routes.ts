import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import {StopgroupsComponent} from "./stopgroups/stopgroups.component";
import { DivisionsListComponent } from './divisions-list/divisions-list.component';

export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'stopgroups', component: StopgroupsComponent},
  {path: 'divisions', component: DivisionsListComponent},
  {path: '', redirectTo: 'login', pathMatch: 'full'},
];



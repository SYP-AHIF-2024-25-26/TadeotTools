import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import {StopgroupsComponent} from "./stopgroups/stopgroups.component";

export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'stopgroups', component: StopgroupsComponent},
  {path: '', redirectTo: 'login', pathMatch: 'full'},
];



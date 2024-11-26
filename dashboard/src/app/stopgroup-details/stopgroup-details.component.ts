import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-stopgroup-details',
  standalone: true,
  imports: [],
  templateUrl: './stopgroup-details.component.html',
  styleUrl: './stopgroup-details.component.css'
})
export class StopgroupDetailsComponent {
  stopgroupId = signal<number>(-1);
}

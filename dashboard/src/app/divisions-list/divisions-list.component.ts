import { Component, inject, OnInit, signal } from '@angular/core';
import { DivisionService } from '../division.service';
import { Division } from '../types';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-divisions-list',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './divisions-list.component.html',
  styleUrl: './divisions-list.component.css',
})
export class DivisionsListComponent implements OnInit {
  constructor(private router: Router) {}
  divisions = signal<Division[]>([]);

  private service: DivisionService = inject(DivisionService);
  async ngOnInit() {
    this.divisions.set(await this.service.getDivisions());
  }

  async deleteDivision(divisionId: number) {
    await this.service.deleteDivision(divisionId);
    this.divisions.set(await this.service.getDivisions());
  }
}

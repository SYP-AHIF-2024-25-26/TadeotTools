import { Component, inject, OnInit, signal } from '@angular/core';
import { Division, DivisionService } from '../division.service';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar.component";

@Component({
  selector: 'app-divisions-list',
  standalone: true,
  imports: [RouterModule, NavbarComponent],
  templateUrl: './divisions-list.component.html',
  styleUrl: './divisions-list.component.css'
})
export class DivisionsListComponent implements OnInit {
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

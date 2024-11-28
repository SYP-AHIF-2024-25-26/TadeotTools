import { Component, inject, signal } from '@angular/core';
import { StopService } from '../stop.service';
import { BASE_URL } from '../app.config';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Division } from '../types';
import { DivisionService } from '../division.service';

@Component({
  selector: 'app-stop-details',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './stop-details.component.html',
  styleUrl: './stop-details.component.css',
})
export class StopDetailsComponent {
  private service: StopService = inject(StopService);
  private divisionService: DivisionService = inject(DivisionService);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private router: Router = inject(Router);

  baseUrl = inject(BASE_URL);

  stopId = signal<number>(-1);
  name = signal<string>('');
  description = signal<string>('');
  roomNr = signal<string>('');
  stopGroupId = signal<number | null>(null);
  divisionId = signal<number>(5); // general as default

  divisions = signal<Division[]>([]);

  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadDivisions();
    this.route.queryParams.subscribe((params) => {
      this.stopId.set(params['id'] || -1);
      this.name.set(params['name'] || '');
      this.description.set(params['description'] || '');
      this.roomNr.set(params['roomNr'] || '');
      this.stopGroupId.set(params['stopGroupID'] || null);
    });
    this.divisionService.getDivisions().then((divisions) => {
      this.divisions.set(divisions);
    });
  }
  async loadDivisions() {
    this.divisions.set(await this.divisionService.getDivisions());
  }

  async submitStopDetail() {
    if (this.stopId() === -1) {
      await this.service.addStop({
        name: this.name(),
        description: this.description(),
        roomNr: this.roomNr(),
        divisionID: this.divisionId(),
      });
    } else {
      await this.service.updateStop({
        stopID: this.stopId(),
        name: this.name(),
        description: this.description(),
        roomNr: this.roomNr(),
        stopGroupID: this.stopGroupId() === -1 ? null : this.stopGroupId(),
        divisionID: this.divisionId(),
      });
    }
    this.router.navigate(['/stopgroups']);
  }

  async deleteAndGoBack() {
    await this.service.deleteStop(this.stopId());
    this.router.navigate(['/stopgroups']);
  }
}

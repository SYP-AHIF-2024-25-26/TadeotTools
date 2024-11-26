import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StopGroupService } from '../stopgroup.service';

@Component({
  selector: 'app-stopgroup-details',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './stopgroup-details.component.html',
  styleUrl: './stopgroup-details.component.css',
})
export class StopgroupDetailsComponent {
  private service: StopGroupService = inject(StopGroupService);

  stopGroupId = signal<number>(-1);
  name = signal<string>('');
  errorMessage = signal<string>('');
  description = signal<string>('');
  isPublic = signal<boolean>(false);

  constructor(private route: ActivatedRoute, private router: Router) {
    this.route.queryParams.subscribe((params) => {
      this.name.set(params['name'] || '');
      this.description.set(params['description'] || '');
      this.stopGroupId.set(params['stopGroupID'] || null);
      this.isPublic.set(params['isPublic'] || false);
    });
  }

  async submitStopGroupDetails() {
    if (this.stopGroupId() === -1) {
      await this.service.addStopGroup({
        name: this.name(),
        description: this.description(),
        isPublic: this.isPublic(),
      });
    } else {
      await this.service.updateStopGroup({
        stopGroupID: this.stopGroupId(),
        name: this.name(),
        description: this.description(),
        isPublic: this.isPublic(),
      });
    }
    this.router.navigate(['/divisions']);
  }

  deleteAndGoBack() {}
}

import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { StopGroupService } from '../stopgroup.service';
import { isValid } from '../utilfunctions';

@Component({
  selector: 'app-stopgroup-details',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './stopgroup-details.component.html',
  styleUrl: './stopgroup-details.component.css',
})
export class StopgroupDetailsComponent implements OnInit {
  private service: StopGroupService = inject(StopGroupService);
  private router: Router = inject(Router);
  private route: ActivatedRoute = inject(ActivatedRoute);

  stopGroupId = signal<number>(-1);
  name = signal<string>('');
  errorMessage = signal<string>('');
  description = signal<string>('');
  isPublic = signal<boolean>(false);

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.stopGroupId.set(params['id'] || -1);
      console.log('stopGroupId', this.stopGroupId());
      this.name.set(params['name'] || '');
      this.description.set(params['description'] || '');
      let isPublic = params['isPublic'] || "";
      this.isPublic.set(isPublic !== "" && isPublic === "true");
      console.log("test: " + typeof(this.isPublic()));
    });
  }

  isInputValid(): boolean {
    if (!isValid(this.name(), 50)) {
      this.errorMessage.set('Name is invalid, must be less than 50 characters');
      return false;
    }
    if (!isValid(this.description(), 255)) {
      this.errorMessage.set('Description is invalid, must be less than 255 characters');
      return false;
    }
    return true;
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
    this.router.navigate(['/stopgroups']);
  }

  deleteAndGoBack() {
    this.service.deleteStopGroup(this.stopGroupId());
    this.router.navigate(['/stopgroups']);
  }
}

import { Component, inject, Input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DivisionService } from '../division.service';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { BASE_URL } from '../app.config';

@Component({
  selector: 'app-division-details',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './division-details.component.html',
  styleUrl: './division-details.component.css',
})
export class DivisionDetailsComponent {
  private service: DivisionService = inject(DivisionService);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private router: Router = inject(Router);

  baseUrl = inject(BASE_URL);

  divisionId = signal<number>(-1);
  name = signal<string>('');
  color = signal<string>('');

  errorMessage = signal<string | null>(null);
  selectedFile: File | null = null;

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.divisionId.set(params['id'] || -1);
      this.name.set(params['name'] || '');
      this.color.set(params['color'] || '');
    });
    this.color.set(this.color() !== '' ? '#' + this.color().substring(1) : '');
  }

  async onFileChange(event: any) {
    const file: File = event.target.files[0];

    this.errorMessage.set(null);

    if (file) {
      const validFileTypes = ['image/jpeg', 'image/png', 'image/jpg', 'svg']; // add svg
      if (!validFileTypes.includes(file.type)) {
        this.errorMessage.set(
          'Invalid file type. Please upload a JPG, JPEG, or PNG file.'
        );
        this.selectedFile = null;
        return;
      }
      this.selectedFile = file;
    }
  }

  async submitDivisionDetail() {
    if (this.divisionId() === -1) {
      await this.service.addDivision({
        name: this.name(),
        color: this.color(),
      });
    } else {
      await this.service.updateDivision({
        divisionID: this.divisionId(),
        name: this.name(),
        color: this.color(),
      });
    }
    if (this.selectedFile) {
      await this.service.updateDivisionImg(this.divisionId(), this.selectedFile);
    }
    this.router.navigate(['/divisions']);
  }

  async deleteAndGoBack() {
    await this.service.deleteDivision(this.divisionId());
    this.router.navigate(['/divisions']);
  }
}

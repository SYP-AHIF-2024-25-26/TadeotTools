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
  baseUrl = inject(BASE_URL);

 divisionId = signal<number>(-1);
  name = signal<string>('');
  color = signal<string>('');

  newDivision = () => this.divisionId() === -1;

  image = signal<string>('');
  errorMessage = signal<string | null>(null);
  selectedFile: File | null = null;
  byteArray: Uint8Array | null = null;

  constructor(private route: ActivatedRoute) {
    console.log(parseInt(''));
    this.divisionId.set(parseInt(this.route.snapshot.paramMap.get('id') || ''));
    this.name.set(this.route.snapshot.paramMap.get('name') || '');
    this.color.set(this.route.snapshot.paramMap.get('color') || '');
    this.color.set(this.color() !== '' ? '#' + this.color().substring(1) : '');
  }

  onFileChange(event: any) {
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
      this.convertToByteArray(file);
    }
  }

  convertToByteArray(file: File): void {
    const reader = new FileReader();
    reader.onloadend = () => {
      this.byteArray = new Uint8Array(reader.result as ArrayBuffer);
    };
    reader.readAsArrayBuffer(file);
  }

  ngOnInit() {}
  submitDivisionDetail() {
    if (this.newDivision()) {
      this.service.addDivision({
        name: this.name(),
        color: this.color(),
        image: this.image(),
      });
    } else {
      this.service.updateDivision({
        divisionID: this.divisionId(),
        name: this.name(),
        color: this.color(),
        image: this.image(),
      });
    }
  }
}

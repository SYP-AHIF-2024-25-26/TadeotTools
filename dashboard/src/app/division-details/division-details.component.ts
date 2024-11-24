import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DivisionService } from '../division.service';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar.component";

@Component({
  selector: 'app-division-details',
  standalone: true,
  imports: [FormsModule, RouterModule, NavbarComponent],
  templateUrl: './division-details.component.html',
  styleUrl: './division-details.component.css'
})
export class DivisionDetailsComponent {
  private service: DivisionService = inject(DivisionService);

  divisionId = signal<number>(-1);
  name = signal<string>('');
  color = signal<string>('');
  image = signal<string>('');
  newDivision = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  selectedFile: File | null = null;
  byteArray: Uint8Array | null = null;

  constructor(private route: ActivatedRoute) {}

  onFileChange(event: any) {
    const file: File = event.target.files[0];

    this.errorMessage.set(null);

    if (file) {
      const validFileTypes = ['image/jpeg', 'image/png'];
      if (!validFileTypes.includes(file.type)) {
        this.errorMessage.set('Invalid file type. Please upload a JPG, JPEG, or PNG file.');
        this.selectedFile = null; // Reset the selected file
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

  ngOnInit() {
    const id: string | null = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.divisionId.set(parseInt(id));
      this.service.getDivision(this.divisionId()).then(division => {
        this.name.set(division.name);
        this.color.set(division.color);
        this.image.set(division.image || '');
      });
    } else {
      this.newDivision.set(true);
    }
  }
  submitDivisionDetail() {
    if (this.newDivision()) {
      this.service.addDivision({
        name: this.name(),
        color: this.color(),
        image: this.image()
      });
    } else {
      this.service.updateDivision({
        divisionID: this.divisionId(),
        name: this.name(),
        color: this.color(),
        image: this.image()
      });
    }
  }
}

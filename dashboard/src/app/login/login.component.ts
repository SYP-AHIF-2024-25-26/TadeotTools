import { Component, signal } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { Router } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar.component";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NavbarComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  apiKey = signal<string>("");
  constructor(private router: Router) {
    this.apiKey.set(localStorage.getItem('apiKey') || '');
    if (this.apiKey() !== '') {
      this.saveApiKey();
    }
  }

  saveApiKey() {
    if (this.apiKey() !== '') {
      localStorage.setItem('apiKey', this.apiKey());
      this.router.navigate(['/stopgroups']);
    }
  }
}

import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GuideCardComponent } from './guide-card/guide-card.component';
import { NavbarComponent } from './navbar/navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, GuideCardComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TadeoT';
}

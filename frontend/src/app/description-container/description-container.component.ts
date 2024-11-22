import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-description-container',
  standalone: true,
  imports: [],
  templateUrl: './description-container.component.html',
  styleUrl: './description-container.component.css'
})
export class DescriptionContainerComponent {
  @Input() title!: string;
  @Input() description!: string;
}

import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-description-container',
  standalone: true,
  imports: [],
  templateUrl: './description-container.component.html',
  styleUrl: './description-container.component.css'
})
export class DescriptionContainerComponent {
  @Input({required: true}) title!: string;
  @Input({required: true}) description!: string;
  @Input() roomNr: string | undefined;
}

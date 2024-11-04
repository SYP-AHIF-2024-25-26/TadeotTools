import { Component, Input, inject } from '@angular/core';
import { Router, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-breadcrumb',
  standalone: true,
  imports: [
    RouterLinkActive,
  ],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css'
})
export class BreadcrumbComponent {
  @Input() name: string = '';

  protected router = inject(Router);
}

import { Component, inject, Input } from '@angular/core';
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
  protected router = inject(Router);
  @Input() name!: string;
  @Input({required: true}) stopGroupId!: number;
}

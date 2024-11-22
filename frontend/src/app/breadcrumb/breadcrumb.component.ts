import { Component, inject, computed, Input } from '@angular/core';
import { Router, RouterLinkActive } from '@angular/router';
import { ApiFetchService } from '../api-fetch.service';

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

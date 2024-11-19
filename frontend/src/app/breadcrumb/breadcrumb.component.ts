import { Component, Input, inject, computed } from '@angular/core';
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
  protected apiFetchService = inject(ApiFetchService);

  name = computed(() => {
    return this.apiFetchService.getStopGroupName();
  });

}

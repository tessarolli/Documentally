import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})

export class MainLayoutComponent {
  menuItems: MenuItem[];

  constructor() {
    this.menuItems = [
      { label: 'My Files', icon: 'pi pi-fw pi-file', routerLink: '/files' },
      { label: 'Shared With Me', icon: 'pi pi-fw pi-users', routerLink: '/shared' },
    ];
  }
}

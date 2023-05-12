import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Observable } from 'rxjs';
import { AuthenticationState } from '../../authentication/state/authentication.state';
import { Store, select } from '@ngrx/store';
import { AppState } from '../../app.state';
import { selectAuthenticationState } from '../../authentication/state/authentication.selectors';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})

export class MainLayoutComponent {
  menuItems: MenuItem[];
  authenticationState$!: Observable<AuthenticationState | null>;

  constructor(private store: Store<AppState>, private router: Router) {
    this.menuItems = [
      { label: 'My Files', icon: 'pi pi-fw pi-file', routerLink: '/files' },
      { label: 'Shared With Me', icon: 'pi pi-fw pi-users', routerLink: '/shared' },
    ];
  }

  ngOnInit() {
    this.authenticationState$ = this.store.pipe(select(selectAuthenticationState));
    this.authenticationState$.subscribe((state) => {
      if (state && !state.isAuthenticated) {
        this.router.navigate(['/login']);
      }
    });
  }
}

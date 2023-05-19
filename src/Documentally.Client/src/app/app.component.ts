import { Component, OnInit } from '@angular/core';
import { SharedService } from './shared/shared.service';
import { AppState } from './app.state';
import { Store } from '@ngrx/store';
import { selectIsAdmin, selectIsAuthenticated } from './authentication/state/authentication.selectors';
import { Observable } from 'rxjs';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  isAuthenticated$!: Observable<boolean>;
  isAdmin$!: Observable<boolean>;

  constructor(
    private store: Store<AppState>,
    private sharedService: SharedService
  ) { }

  ngOnInit(): void {
    this.sharedService.setPageTitle('Documentally');

    // Subscribe to IsAuthenticated state from AuthenticationState
    this.isAuthenticated$ = this.store.select(selectIsAuthenticated);
    // Subscribe to IsAdmin state from AuthenticationState
    this.isAdmin$ = this.store.select(selectIsAdmin);
  }

  getPageTitle(): string {
    return this.sharedService.getPageTitle();
  }
}

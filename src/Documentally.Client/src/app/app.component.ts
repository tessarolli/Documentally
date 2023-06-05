import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AppState } from './app.state';
import { Store } from '@ngrx/store';
import { selectIsAdmin, selectIsAuthenticated } from './authentication/state/authentication.selectors';
import { Observable } from 'rxjs';
import { selectPageTitle } from './core/state/root.selectors';
import { SetTitle } from './core/state/root.actions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class AppComponent {
  isAuthenticated$: Observable<boolean>;
  isAdmin$: Observable<boolean>;
  pageTitle$: Observable<string>;

  constructor(
    private store: Store<AppState>,
  ) {
    this.store.dispatch(SetTitle({ title: 'Documentally' }));

    // Subscribe to IsAuthenticated state from AuthenticationState
    this.isAuthenticated$ = this.store.select(selectIsAuthenticated);

    // Subscribe to IsAdmin state from AuthenticationState
    this.isAdmin$ = this.store.select(selectIsAdmin);

    // Subscribe to pageTitle state from RootState
    this.pageTitle$ = this.store.select(selectPageTitle);
  }

}

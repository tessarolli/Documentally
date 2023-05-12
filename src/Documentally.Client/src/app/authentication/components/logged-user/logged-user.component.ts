import { Component } from '@angular/core';
import { Observable, } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { AuthenticationState } from '../../state/authentication.state';
import { selectAuthenticationState } from '../../state/authentication.selectors';
import { Logout } from '../../state/authentication.actions';

@Component({
  selector: 'app-logged-user',
  templateUrl: './logged-user.component.html',
  styleUrls: ['./logged-user.component.css']
})
export class LoggedUserComponent {
  authenticationState$!: Observable<AuthenticationState | null>;

  constructor(private store: Store<AppState>) { }

  ngOnInit() {
    this.authenticationState$ = this.store.pipe(select(selectAuthenticationState));
  }

  logout(): void {
    this.store.dispatch(Logout());
  }
}

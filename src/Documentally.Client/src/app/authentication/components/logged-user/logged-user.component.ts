import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, } from 'rxjs';
import { Store } from '@ngrx/store';
import { PopoverController } from '@ionic/angular';
import { AppState } from '../../../app.state';
import { Logout } from '../../state/authentication.actions';
import { AuthenticatedUser } from '../../models/authenticatedUser.model';
import { selectAuthenticatedUser } from '../../state/authentication.selectors';
import { UserRole } from '../../enums/UserRole.enum';

@Component({
  selector: 'app-logged-user',
  templateUrl: './logged-user.component.html',
  styleUrls: ['./logged-user.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoggedUserComponent implements OnInit {
  authenticatedUser$!: Observable<AuthenticatedUser | null>;
  userRole!: string;

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private popoverController: PopoverController) { }

  ngOnInit() {
    this.authenticatedUser$ = this.store.select(selectAuthenticatedUser);
    this.authenticatedUser$.subscribe(user => {
      if (user)
        this.userRole = UserRole[user.role];
    });
  }

  logout(): void {
    this.popoverController.dismiss('loggedUserPopOver');

    this.store.dispatch(Logout());

    this.router.navigate(['/login']);
  }
}

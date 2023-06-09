import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ViewDidEnter, ViewWillLeave } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { ClearError, Login } from '../../state/authentication.actions';
import { selectError, selectIsAuthenticated, selectIsLoading } from '../../state/authentication.selectors';
import { Subject, takeUntil } from 'rxjs';
import { SetTitle } from '../../../core/state/root.actions';
import { AlertService } from '../../../core/services/alert.service';
@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class LoginPageComponent implements ViewDidEnter, ViewWillLeave {
  loginForm: FormGroup;
  destroy$ = new Subject<void>();
  isLoading$ = this.store.select(selectIsLoading);

  constructor(
    private alert: AlertService,
    private router: Router,
    private store: Store<AppState>,
  ) {
    // Define the Reactive Forms for the Login Form
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    });
  }

  ionViewDidEnter(): void {
    this.store.dispatch(SetTitle({ title: 'Login' }));

    // Subscribe to Error state from AuthenticationState
    this.store.select(selectError)
      .pipe(takeUntil(this.destroy$))
      .subscribe((error) => {
        if (error) {
          // Clears the error message, so that it can rethrow the alert again
          this.store.dispatch(ClearError());

          // presents the Alert message to the user
          this.alert.Error(error)
            .catch(console.log);
        }
      });

    // Subscribe to IsAuthenticated state from AuthenticationState
    this.store.select(selectIsAuthenticated)
      .pipe(takeUntil(this.destroy$))
      .subscribe((isAuthenticated) => {
        if (isAuthenticated) {
          this.router.navigate(['/'])
            .catch(console.log);
        }
      });
  }

  ionViewWillLeave(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // The Action Sheet buttons declaration
  public actionSheetButtons = [
    {
      text: 'User',
      data: 'user',
    },
    {
      text: 'Manager',
      data: 'manager',
    },
    {
      text: 'Admin',
      data: 'admin',
    },
  ];

  // Login Method
  login(): void {
    if (this.loginForm.invalid) {
      return;
    }

    const email = this.loginForm.controls['email'].value;
    const password = this.loginForm.controls['password'].value;

    this.store.dispatch(Login({ email, password }));
  }

  // Method that is called when user clicks on a Quick Login Action Sheet
  async quickLogin(action: any): Promise<void> {
      const selectedAction = action?.detail?.data;
      if (selectedAction) {
        this.loginForm.controls['email'].setValue(selectedAction + '@documentally.com');
        this.loginForm.controls['password'].setValue(selectedAction);
        this.login();
      }
  }
}

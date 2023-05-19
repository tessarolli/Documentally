import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../shared/shared.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { selectError, selectIsAuthenticated, selectIsLoading } from '../../state/authentication.selectors';
import { AlertController, ViewDidEnter, ViewWillLeave } from '@ionic/angular';
import { AppState } from '../../../app.state';
import { Store } from '@ngrx/store';
import { ClearError, Register } from '../../state/authentication.actions';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterPageComponent implements ViewDidEnter, ViewWillLeave {
  registerForm: FormGroup;
  isLoading$ = this.store.select(selectIsLoading);
  destroy$ = new Subject<void>();

  constructor(
    private alertController: AlertController,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store<AppState>,
    private sharedService: SharedService
  ) {
    this.sharedService.setPageTitle('Register');

    // Define the Reactive Forms for the Register Form
    this.registerForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    });
  }

  ionViewDidEnter(): void {
    // Subscribe to Error state from AuthenticationState
    this.store.select(selectError)
      .pipe(takeUntil(this.destroy$))
      .subscribe((error) => {
        if (error) {
          // Clears the error message, so that it can rethrow the alert again
          this.store.dispatch(ClearError());

          // presents the Alert message to the user
          this.presentAlert(error);
        }
      });

    // Subscribe to IsAuthenticated state from AuthenticationState
    this.store.select(selectIsAuthenticated)
      .pipe(takeUntil(this.destroy$))
      .subscribe((isAuthenticated) => {
        if (isAuthenticated) {
          this.router.navigate(['/']);
        }
      });
  }

  ionViewWillLeave(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Method for displaying an alert message for the user
  async presentAlert(msg: string) {
    const alert = await this.alertController.create({
      header: 'Message',
      message: msg,
      buttons: ['OK'],
    });

    await alert.present();
  }

  // Register Method
  register(): void {
    if (this.registerForm.invalid) {
      return;
    }

    const firstName = this.registerForm.controls['firstName'].value;
    const lastName = this.registerForm.controls['lastName'].value;
    const email = this.registerForm.controls['email'].value;
    const password = this.registerForm.controls['password'].value;

    this.store.dispatch(Register({ firstName, lastName, email, password }));
  }
}


import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AlertController } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { ClearError, Login } from '../../state/authentication.actions';
import { selectError, selectIsAuthenticated, selectIsLoading } from '../../state/authentication.selectors';
import { SharedService } from '../../../shared/shared.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})

export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;
  isLoading$ = this.store.select(selectIsLoading);

  constructor(
    private alertController: AlertController,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store<AppState>,
    private sharedService: SharedService
  ) { }

  ngOnInit(): void {
    // Subscribe to ActivatedRoute in order to update the Title in the root component
    this.route.params.subscribe(() => {
      this.sharedService.setPageTitle('Login');
    });

    // Define the Reactive Forms for the Login Form
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required)
    });

    // Subscribe to Error state from AuthenticationState
    this.store.select(selectError).subscribe((error) => {
      if (error) {
        // Clears the error message, so that it can rethrow the alert again
        this.store.dispatch(ClearError());

        // presents the Alert message to the user
        this.presentAlert(error);
      }
    });

    // Subscribe to IsAuthenticated state from AuthenticationState
    this.store.select(selectIsAuthenticated).subscribe((isAuthenticated) => {
      if (isAuthenticated) {
        this.router.navigate(['/']);
      }
    });
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

  // Method for displaying an alert message for the user
  async presentAlert(msg: string) {
    const alert = await this.alertController.create({
      header: 'Message',
      message: msg,
      buttons: ['OK'],
    });

    await alert.present();
  }

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

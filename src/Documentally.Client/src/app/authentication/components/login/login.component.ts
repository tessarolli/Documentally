import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { Login } from '../../state/authentication.actions';
import { FormControl, FormGroup, NgForm } from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email!: string;
  password!: string;

  constructor(private store: Store<AppState>) {  }

  login(): void {
    this.store.dispatch(Login({ email: this.email, password: this.password }));
  }
}

import { NgModule } from '@angular/core';

import { SharedModule } from '../../../shared/shared.module';
import { LoginPageComponent } from './login-page.component';

@NgModule({
  imports: [
    SharedModule,
  ],
  declarations: [
    LoginPageComponent
  ]
})
export class LoginPageModule { }

import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

// Routing
import { AuthenticationRoutingModule } from './authentication-routing.module';

// Components
import { LoggedUserComponent } from './components/logged-user/logged-user.component';

// Angular HTTP Interceptor
import { HttpAuthorizationInterceptor } from '../core/http.authorization.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

// NgRx
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AuthenticationReducer } from './state/authentication.reducer';
import { AuthenticationEffects } from './state/authentication.effects';

// Services
import { AuthenticationService } from './authentication.service';

// Modules
import { LoginPageModule } from './pages/login-page/login-page.module';
import { RegisterPageModule } from './pages/register-page/register-page.module';
import { RecoverPasswordPageModule } from './pages/recover-password-page/recover-password-page.module';

@NgModule({
  imports: [
    StoreModule.forFeature('authentication', AuthenticationReducer),
    EffectsModule.forFeature([AuthenticationEffects]),
    SharedModule,
    AuthenticationRoutingModule,
    LoginPageModule,
    RegisterPageModule,
    RecoverPasswordPageModule,
  ],
  declarations: [
    LoggedUserComponent
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpAuthorizationInterceptor, multi: true },
    AuthenticationService,
  ],
  exports: [
    LoggedUserComponent
  ]
})
export class AuthenticationModule { }

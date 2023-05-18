import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

// Routing
import { AuthenticationRoutingModule } from './authentication-routing.module';

// Components
import { LoggedUserComponent } from './components/logged-user/logged-user.component';

// Angular HTTP Interceptor
import { AuthenticationInterceptor } from './interceptors/authentication-interceptor';
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
import { RecoverPasswordPageComponent } from './pages/recover-password-page/recover-password-page.component';

@NgModule({
  imports: [
    StoreModule.forFeature('authentication', AuthenticationReducer),
    EffectsModule.forRoot([AuthenticationEffects]),
    AuthenticationRoutingModule,
    LoginPageModule,
    SharedModule,
  ],
  declarations: [
    LoggedUserComponent,
    RecoverPasswordPageComponent,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    AuthenticationService,
  ],
  exports: [
    LoggedUserComponent,
    SharedModule,
    LoginPageModule,
  ]
})
export class AuthenticationModule { }

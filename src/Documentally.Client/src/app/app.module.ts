
// Angular and Local Project Module Imports
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationInterceptor } from './authentication/interceptors/authentication-interceptor';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// 3rd Party Imports
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { ToastModule } from 'primeng/toast';
import { AvatarModule } from 'primeng/avatar';
import { OverlayModule } from 'primeng/overlay';
import { CardModule } from 'primeng/card';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';

// Components Imports
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { LoggedUserComponent } from './authentication/components/logged-user/logged-user.component';
import { LoginComponent } from './authentication/components/login/login.component';

// Other Imports
import { AuthenticationReducer } from './authentication/state/authentication.reducer';
import { AuthenticationService } from './authentication/authentication.service';
import { AuthenticationEffects } from './authentication/state/authentication.effects';

@NgModule({
  declarations: [
    MainLayoutComponent,
    LoggedUserComponent,
    LoginComponent,
  ],
  imports: [
    StoreModule.forRoot({}),
    StoreModule.forFeature('authentication', AuthenticationReducer),
    EffectsModule.forRoot([AuthenticationEffects]),
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MenubarModule,
    ToastModule,
    ButtonModule,
    AvatarModule,
    OverlayModule,
    CardModule,
    OverlayPanelModule,
    InputTextModule,
    PasswordModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    MessageService,
    AuthenticationService,
  ],
  bootstrap: [MainLayoutComponent]
})
export class AppModule { }

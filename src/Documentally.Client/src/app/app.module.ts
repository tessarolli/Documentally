
// Angular and Local Project Module Imports
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationInterceptor } from './interceptors/authentication-interceptor';

// PrimeNg Imports
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { ToastModule } from 'primeng/toast';
import { AvatarModule } from 'primeng/avatar';
import { OverlayModule } from 'primeng/overlay';
import { CardModule } from 'primeng/card';
import { OverlayPanelModule } from 'primeng/overlaypanel';

// Components Imports
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { LoggedUserComponent } from './components/account/logged-user/logged-user.component';

// Services Imports
import { UserService } from './services/user/user.service';
import { AuthenticationService } from './services/authentication/authentication.service';

@NgModule({
  declarations: [
    MainLayoutComponent,
    LoggedUserComponent
  ],
  imports: [
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
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthenticationInterceptor, multi: true },
    MessageService,
    AuthenticationService,
    UserService,
  ],
  bootstrap: [MainLayoutComponent]
})
export class AppModule { }

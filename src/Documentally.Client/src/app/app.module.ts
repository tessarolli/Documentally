import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Components
import { AppComponent } from './app.component';

// Local Modules
import { AppRoutingModule } from './app-routing.module';
import { AuthenticationModule } from './authentication/authentication.module';

// NgRx
import { StoreModule } from '@ngrx/store';

// Ionic
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    StoreModule.forRoot(),
    IonicModule.forRoot(),
    AppRoutingModule,
    AuthenticationModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
  ],
  providers: [
  ],
  bootstrap: [
    AppComponent
  ],
  exports: [
  ]
})

export class AppModule { }

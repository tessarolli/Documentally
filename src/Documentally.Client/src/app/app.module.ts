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
import { metaReducers, reducers } from './app.state';

// Ionic
import { IonicModule } from '@ionic/angular';
import { EffectsModule } from '@ngrx/effects';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    IonicModule.forRoot(),
    EffectsModule.forRoot(),
    StoreModule.forRoot(reducers, { metaReducers }),
    AppRoutingModule,
    AuthenticationModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
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

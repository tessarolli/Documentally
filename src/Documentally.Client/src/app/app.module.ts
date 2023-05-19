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


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    StoreModule.forRoot(reducers, {metaReducers}),
    IonicModule.forRoot(),
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

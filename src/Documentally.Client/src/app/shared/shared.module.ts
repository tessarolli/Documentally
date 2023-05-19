import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// NgRx imports
import { EffectsModule } from '@ngrx/effects';

// Ionic
import { IonicModule } from '@ionic/angular';

// Services
import { SharedService } from './shared.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EffectsModule,
    RouterModule,
  ],
  providers: [
    SharedService
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EffectsModule,
    RouterModule,
    IonicModule
  ]
})
export class SharedModule { }

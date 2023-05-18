import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// NgRx imports
import { EffectsModule } from '@ngrx/effects';

// Ionic
import { IonicModule } from '@ionic/angular';

// Services
import { SharedService } from '../services/shared.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EffectsModule,
  ],
  providers: [
    SharedService
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EffectsModule,
    IonicModule
  ]
})
export class SharedModule { }

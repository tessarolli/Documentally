import { NgModule } from '@angular/core';
import { RegisterPageComponent } from './register-page.component';
import { SharedModule } from '../../../shared/shared.module';

@NgModule({
  declarations: [
    RegisterPageComponent
  ],
  imports: [
    SharedModule
  ],
  exports: [
    RegisterPageComponent
  ]
})
export class RegisterPageModule { }

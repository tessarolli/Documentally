import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { MyDocumentsPageComponent } from './my-documents-page.component';

@NgModule({
  declarations: [
    MyDocumentsPageComponent
  ],
  imports: [
    SharedModule,
  ]
})
export class MyDocumentsPageModule { }

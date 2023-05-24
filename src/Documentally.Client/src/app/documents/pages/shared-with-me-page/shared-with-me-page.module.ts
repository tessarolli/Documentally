import { NgModule } from '@angular/core';
import { SharedWithMePageComponent } from './shared-with-me-page.component';
import { SharedModule } from '../../../shared/shared.module';
import { DocumentsModule } from '../../documents.module';

@NgModule({
  declarations: [
    SharedWithMePageComponent,
  ],
  imports: [
    SharedModule,
    DocumentsModule
  ]
})
export class SharedWithMePageModule { }

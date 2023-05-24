import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { MyDocumentsPageComponent } from './my-documents-page.component';
import { DocumentsModule } from '../../documents.module';
import { StoreModule } from '@ngrx/store';
import { MyDocumentsPageReducer } from './my-documents-page.reducer';
import { EffectsModule } from '@ngrx/effects';
import { MyDocumentsPageEffects } from './my-documents-page.effects';

@NgModule({
  declarations: [
    MyDocumentsPageComponent,
  ],
  imports: [
    SharedModule,
    DocumentsModule,
    StoreModule.forFeature('myDocuments', MyDocumentsPageReducer),
    EffectsModule.forFeature([MyDocumentsPageEffects]),
  ]

})
export class MyDocumentsPageModule { }

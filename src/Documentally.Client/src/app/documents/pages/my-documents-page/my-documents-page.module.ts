import { NgModule } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { MyDocumentsPageComponent } from './my-documents-page.component';
import { StoreModule } from '@ngrx/store';
import { MyDocumentsPageReducer } from './state/my-documents-page.reducer';
import { EffectsModule } from '@ngrx/effects';
import { MyDocumentsPageEffects } from './state/my-documents-page.effects';
import { DocumentsComponentsModule } from '../../components/documents-components.module';

@NgModule({
  imports: [
    StoreModule.forFeature('myDocuments', MyDocumentsPageReducer),
    EffectsModule.forFeature([MyDocumentsPageEffects]),
    SharedModule,
    DocumentsComponentsModule
  ],
  declarations: [
    MyDocumentsPageComponent,
  ],
})
export class MyDocumentsPageModule { }

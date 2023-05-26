import { NgModule } from '@angular/core';
import { SharedWithMePageComponent } from './shared-with-me-page.component';
import { SharedModule } from '../../../shared/shared.module';
import { DocumentsComponentsModule } from '../../components/documents-components.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { SharedWithMePageReducer } from './state/shared-with-me-page.reducer';
import { SharedWithMePageEffects } from './state/shared-with-me-page.effects';

@NgModule({
  imports: [
    StoreModule.forFeature('sharedWithMe', SharedWithMePageReducer),
    EffectsModule.forFeature([SharedWithMePageEffects]),
    SharedModule,
    DocumentsComponentsModule,
  ],
  declarations: [
    SharedWithMePageComponent,
  ],
})
export class SharedWithMePageModule { }

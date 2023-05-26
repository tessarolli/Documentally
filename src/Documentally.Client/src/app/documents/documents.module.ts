import { NgModule } from '@angular/core';
import { DocumentsRoutingModule } from './documents-routing.module';
import { SharedModule } from '../shared/shared.module';
import { DocumentsService } from './services/documents.service';
import { SharedWithMePageModule } from './pages/shared-with-me-page/shared-with-me-page.module';
import { MyDocumentsPageModule } from './pages/my-documents-page/my-documents-page.module';
import { DocumentsComponentsModule } from './components/documents-components.module';

@NgModule({
  declarations: [
  ],
  imports: [
    SharedModule,
    DocumentsRoutingModule,
    DocumentsComponentsModule,
    MyDocumentsPageModule,
    SharedWithMePageModule,
  ],
  providers: [
    DocumentsService
  ],
  exports: [
  ],
})
export class DocumentsModule { }

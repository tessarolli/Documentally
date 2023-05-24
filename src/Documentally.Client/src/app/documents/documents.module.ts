import { NgModule } from '@angular/core';
import { DocumentsRoutingModule } from './documents-routing.module';
import { DocumentsListComponent } from './components/documents-list/documents-list.component';
import { SharedModule } from '../shared/shared.module';
import { DocumentsService } from './services/documents.service';

@NgModule({
  declarations: [
    DocumentsListComponent,
  ],
  imports: [
    SharedModule,
    DocumentsRoutingModule,
  ],
  providers: [
    DocumentsService
  ],
  exports: [
    DocumentsListComponent,
  ],
})
export class DocumentsModule { }

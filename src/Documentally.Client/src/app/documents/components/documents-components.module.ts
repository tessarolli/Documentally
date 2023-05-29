import { NgModule } from "@angular/core";
import { DocumentsListComponent } from "./documents-list/documents-list.component";
import { SharedModule } from "../../shared/shared.module";
import { UploadDocumentModalComponent } from './upload-document-modal/upload-document-modal.component';
import { NgxDropzoneModule } from 'ngx-dropzone';

@NgModule({
  imports: [
    SharedModule,
    NgxDropzoneModule
  ],
  declarations: [
    DocumentsListComponent,
    UploadDocumentModalComponent
  ],
  exports: [
    DocumentsListComponent,
    UploadDocumentModalComponent
  ],
})
export class DocumentsComponentsModule { }

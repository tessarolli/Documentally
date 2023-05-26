import { NgModule } from "@angular/core";
import { DocumentsListComponent } from "./documents-list/documents-list.component";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    DocumentsListComponent
  ],
  exports: [
    DocumentsListComponent
  ],
})
export class DocumentsComponentsModule { }

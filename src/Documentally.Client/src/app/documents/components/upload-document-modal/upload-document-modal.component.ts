import { Component, EventEmitter, Output } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { DocumentsService } from '../../services/documents.service';
import { catchError } from 'rxjs';
import { AlertService } from '../../../core/services/alert.service';
import { HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { AppState } from '../../../app.state';
import { Store } from '@ngrx/store';
import { selectAuthenticatedUser } from '../../../authentication/state/authentication.selectors';

@Component({
  selector: 'app-upload-document-modal',
  templateUrl: './upload-document-modal.component.html',
  styleUrls: ['./upload-document-modal.component.css']
})
export class UploadDocumentModalComponent {
  selectedFile: File | null = null;
  description = '';
  category = '';
  progress = 0;
  message = '';
  @Output() public uploadFinished = new EventEmitter();

  constructor(
    private modalController: ModalController,
    private documentsService: DocumentsService,
    private alert: AlertService,
    private store: Store<AppState>
  ) {
  }

  dismissModal() {
    this.modalController.dismiss()
      .catch(console.log);
  }

  uploadDocument() {
    if (!this.selectedFile) {
      return;
    }

    this.store.select(selectAuthenticatedUser).subscribe(user => {
      if (user && this.selectedFile) {

        const formData = new FormData();

        formData.append('UserId', user.id.toString());
        formData.append('FileName', this.selectedFile.name);
        formData.append('Description', this.description);
        formData.append('Category', this.category);
        formData.append('File', this.selectedFile, this.selectedFile.name);


        this.documentsService.UploadDocument(formData)
          .pipe(catchError(async (error) => this.alert.Error(error)))
          .subscribe({
            next: (event) => {
              if (event.type === HttpEventType.UploadProgress)
                this.progress = Math.round(100 * event.loaded / event.total);
              else if (event.type === HttpEventType.Response) {
                this.message = 'Upload success.';
                this.uploadFinished.emit(event.body);

                // Dispatch (DocumentUploaded({ document }))
              }
            },
            error: (err: HttpErrorResponse) => console.log(err)
          });


        this.selectedFile = null;

        this.modalController.dismiss()
          .catch(console.log);
      }
    })
  }

  onFileSelected(event: { addedFiles: (File | null)[]; }) {
    this.selectedFile = event.addedFiles[0];
  }

  onRemove() {
    this.selectedFile = null;
  }
}

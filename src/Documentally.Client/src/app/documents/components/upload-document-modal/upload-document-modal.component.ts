import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { DocumentsService } from '../../services/documents.service';
import { catchError } from 'rxjs';
import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-upload-document-modal',
  templateUrl: './upload-document-modal.component.html',
  styleUrls: ['./upload-document-modal.component.css']
})
export class UploadDocumentModalComponent implements OnInit {
  selectedFile: File | null = null;
  description: string = '';
  category: string = '';

  constructor(
    private modalController: ModalController,
    private documentsService: DocumentsService,
    private alert: AlertService) { }

  ngOnInit() { }

  dismissModal() {
    this.modalController.dismiss();
  }

  uploadDocument() {
    if (!this.selectedFile) {
      return;
    }

    const formData = new FormData();
    formData.append('UserId', '123456');
    formData.append('FileName', this.selectedFile.name);
    formData.append('Description', this.description);
    formData.append('Category', this.category);
    formData.append('File', this.selectedFile,);


    this.documentsService.UploadDocument(formData)
      .pipe(catchError(async (error) => this.alert.Error(error)))
      .subscribe((event: any) => {

      });


    this.selectedFile = null;

    this.modalController.dismiss();
  }

  onFileSelected(event: any) {
    this.selectedFile = event.addedFiles[0];
  }

  onRemove() {
    this.selectedFile = null;
  }
}

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentModel } from '../../models/document.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { FileIconService } from '../../../core/services/file-icon.service';
import { selectIsAdmin } from '../../../authentication/state/authentication.selectors';
import { AlertController } from '@ionic/angular';
import { selectMyDocuments, selectMyDocumentsError, selectMyDocumentsLoading } from '../../pages/my-documents-page/my-documents-page.selectors';

@Component({
  selector: 'app-documents-list',
  templateUrl: './documents-list.component.html',
  styleUrls: ['./documents-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class DocumentsListComponent {
  isAdmin$: Observable<boolean>;
  documents$: Observable<DocumentModel[]>;
  loading$: Observable<boolean>;
  error$: Observable<string>;

  constructor(
    private store: Store<AppState>,
    private fileIconService: FileIconService,
    private alertController: AlertController
  ) {
    this.isAdmin$ = this.store.select(selectIsAdmin);
    this.documents$ = this.store.select(selectMyDocuments);
    this.loading$ = this.store.select(selectMyDocumentsLoading);
    this.error$ = this.store.select(selectMyDocumentsError);
  }

  getUserName(userId: number) {
    return 'User Name';
  }

  getFileIcon(fileName: string): string {
    return this.fileIconService.getFileIcon(fileName);
  }

  async showConfirmationDialog(document: DocumentModel) {
    const alert = await this.alertController.create({
      header: 'Confirm Deletion',
      message: `Are you sure you want to delete ${document.Name}?`,
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
        },
        {
          text: 'Delete',
          handler: () => {
            this.deleteDocument(document);
          },
        },
      ],
    });

    await alert.present();
  }

  deleteDocument(document: DocumentModel) {
    console.log('deleted document:' + document.Name);
  }
}

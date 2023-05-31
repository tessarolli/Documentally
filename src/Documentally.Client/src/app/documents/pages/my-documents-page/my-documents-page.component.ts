import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ModalController, ViewWillEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { LoadMyDocuments } from './state/my-documents-page.actions';
import { Observable } from 'rxjs';
import { DocumentModel } from '../../models/document.model';
import { selectMyDocuments, selectMyDocumentsError, selectMyDocumentsLoading } from './state/my-documents-page.selectors';
import { selectCanUpload } from '../../../authentication/state/authentication.selectors';
import { UploadDocumentModalComponent } from '../../components/upload-document-modal/upload-document-modal.component';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyDocumentsPageComponent implements ViewWillEnter {
  documents$: Observable<DocumentModel[]>;
  isLoading$: Observable<boolean>;
  error$: Observable<string>;
  canUpload$: Observable<boolean>;

  constructor(
    private store: Store<AppState>,
    private modalController: ModalController
  ) {
    this.documents$ = this.store.select(selectMyDocuments);
    this.isLoading$ = this.store.select(selectMyDocumentsLoading);
    this.error$ = this.store.select(selectMyDocumentsError);
    this.canUpload$ = this.store.select(selectCanUpload);
  }

  ionViewWillEnter(): void {
    this.store.dispatch(SetTitle({ title: 'My Documents' }));

    this.store.dispatch(LoadMyDocuments());
  }

  async openUploadModal() {

    const modal = await this.modalController.create({
      component: UploadDocumentModalComponent
    });

    return await modal.present();
  }
}

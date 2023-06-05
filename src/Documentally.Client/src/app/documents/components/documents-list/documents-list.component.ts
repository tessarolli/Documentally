import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { DocumentModel } from '../../models/document.model';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { FileIconService } from '../../../core/services/file-icon.service';
import { selectIsAdmin } from '../../../authentication/state/authentication.selectors';
import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-documents-list',
  templateUrl: './documents-list.component.html',
  styleUrls: ['./documents-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class DocumentsListComponent {
  @Input() documents: DocumentModel[] | null = [];
  @Input() isLoading: boolean | null = false;
  @Input() error: string | null = '';
  isAdmin$: Observable<boolean>;

  constructor(
    private store: Store<AppState>,
    private fileIconService: FileIconService,
    private alert: AlertService
  ) {
    this.isAdmin$ = this.store.select(selectIsAdmin);
  }

  getUserName(userId: number) {
    return 'User Name' + userId;
  }

  getFileIcon(fileName: string): string {
    return this.fileIconService.getFileIcon(fileName);
  }

  async showConfirmationDialog(document: DocumentModel) {
    await this.alert.Confirmation(
      `Are you sure you want to delete ${document.name}?`,
      () => this.deleteDocument(document)
    );
  }

  deleteDocument(document: DocumentModel) {
    console.log('deleted document:' + document.name);
  }
}

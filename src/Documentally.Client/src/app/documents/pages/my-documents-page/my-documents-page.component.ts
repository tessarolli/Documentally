import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewDidEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { selectIsAdmin } from '../../../authentication/state/authentication.selectors';
import { Observable } from 'rxjs';
import { DocumentModel } from '../../models/document.model';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyDocumentsPageComponent implements ViewDidEnter {
  isAdmin$: Observable<boolean>;
  documents: DocumentModel[] = [
    {
      Id: 1,
      OwnerId: 123,
      Name: "Sample Document",
      Description: "This is a sample document",
      Category: "Sample Category",
      Size: 1024,
      BlobUrl: "https://example.com/documents/sample-document.pdf",
      CloudFileName: "sample-document.pdf",
      SharedGroupIds: [456, 789],
      SharedUserIds: [234, 567],
      PostedAtUtc: new Date("2023-05-22T10:30:00Z"),
    }
  ];

  constructor(
    private store: Store<AppState>,
  ) {
    // Subscribe to IsAdmin state from AuthenticationState
    this.isAdmin$ = this.store.select(selectIsAdmin);
  }

  ionViewDidEnter(): void {
    this.store.dispatch(SetTitle({ title: 'My Documents' }));
  }
}

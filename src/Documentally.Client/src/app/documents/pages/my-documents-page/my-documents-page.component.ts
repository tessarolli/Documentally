import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewWillEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { LoadMyDocuments } from './state/my-documents-page.actions';
import { Observable } from 'rxjs';
import { DocumentModel } from '../../models/document.model';
import { selectMyDocuments, selectMyDocumentsError, selectMyDocumentsLoading } from './state/my-documents-page.selectors';

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

  constructor(
    private store: Store<AppState>,
  ) {
    this.documents$ = this.store.select(selectMyDocuments);
    this.isLoading$ = this.store.select(selectMyDocumentsLoading);
    this.error$ = this.store.select(selectMyDocumentsError);
  }

  ionViewWillEnter(): void {
    this.store.dispatch(SetTitle({ title: 'My Documents' }));

    this.store.dispatch(LoadMyDocuments());
  }
}

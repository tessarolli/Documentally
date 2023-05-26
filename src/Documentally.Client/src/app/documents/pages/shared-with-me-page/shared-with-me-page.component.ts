import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewWillEnter } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { SetTitle } from '../../../core/state/root.actions';
import { DocumentModel } from '../../models/document.model';
import { Observable } from 'rxjs';
import { selectSharedWithMe, selectSharedWithMeError, selectSharedWithMeLoading } from './state/shared-with-me-page.selectors';
import { LoadSharedWithMe } from './state/shared-with-me-page.actions';

@Component({
  selector: 'app-shared-with-me-page',
  templateUrl: './shared-with-me-page.component.html',
  styleUrls: ['./shared-with-me-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class SharedWithMePageComponent implements ViewWillEnter {
  documents$: Observable<DocumentModel[]>;
  isLoading$: Observable<boolean>;
  error$: Observable<string>;

  constructor(
    private store: Store<AppState>,
  ) {
    this.documents$ = this.store.select(selectSharedWithMe);
    this.isLoading$ = this.store.select(selectSharedWithMeLoading);
    this.error$ = this.store.select(selectSharedWithMeError);
  }

  ionViewWillEnter(): void {
    this.store.dispatch(SetTitle({ title: 'Shared With Me' }));

    this.store.dispatch(LoadSharedWithMe());
  }
}

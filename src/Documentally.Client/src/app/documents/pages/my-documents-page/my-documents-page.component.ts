import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewDidEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyDocumentsPageComponent implements ViewDidEnter {

  constructor(
    private store: Store<AppState>,
  ) { }

  ionViewDidEnter(): void {
    this.store.dispatch(SetTitle({ title: 'My Documents' }));
  }
}

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewWillEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyDocumentsPageComponent implements ViewWillEnter {
  constructor(
    private store: Store<AppState>,
  ) { }

  ionViewWillEnter(): void {
    this.store.dispatch(SetTitle({ title: 'My Documents' }));
  }
}

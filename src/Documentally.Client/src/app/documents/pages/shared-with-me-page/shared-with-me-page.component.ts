import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewWillEnter } from '@ionic/angular';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app.state';
import { SetTitle } from '../../../core/state/root.actions';

@Component({
  selector: 'app-shared-with-me-page',
  templateUrl: './shared-with-me-page.component.html',
  styleUrls: ['./shared-with-me-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class SharedWithMePageComponent implements ViewWillEnter {
  constructor(
    private store: Store<AppState>,
  ) { }

  ionViewWillEnter(): void {
    this.store.dispatch(SetTitle({ title: 'Shared With Me' }));
  }
}

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ViewDidEnter } from '@ionic/angular';
import { SetTitle } from '../../../core/state/root.actions';
import { AppState } from '../../../app.state';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-recover-password-page',
  templateUrl: './recover-password-page.component.html',
  styleUrls: ['./recover-password-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecoverPasswordPageComponent implements ViewDidEnter {

  constructor(private store: Store<AppState>) { }

  ionViewDidEnter(): void {
    this.store.dispatch(SetTitle({ title: 'Recover Password' }));
  }

}

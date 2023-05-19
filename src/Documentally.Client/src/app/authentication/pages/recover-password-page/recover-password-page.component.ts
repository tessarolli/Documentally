import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../../shared/shared.service';
import { ViewDidEnter } from '@ionic/angular';

@Component({
  selector: 'app-recover-password-page',
  templateUrl: './recover-password-page.component.html',
  styleUrls: ['./recover-password-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecoverPasswordPageComponent implements ViewDidEnter {

  constructor(private sharedService: SharedService) {
    this.sharedService.setPageTitle('Recover Password');
  }

  ionViewDidEnter(): void {
  }

}

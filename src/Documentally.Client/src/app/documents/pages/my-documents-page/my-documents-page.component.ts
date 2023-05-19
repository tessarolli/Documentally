import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SharedService } from '../../../shared/shared.service';
import { ViewDidEnter } from '@ionic/angular';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyDocumentsPageComponent implements ViewDidEnter {

  constructor(
    private sharedService: SharedService,
  ) {
    this.sharedService.setPageTitle('My Documents');
  }

  ionViewDidEnter(): void {
    this.sharedService.setPageTitle('My Documents');
  }
}

import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../services/shared.service';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css']
})
export class MyDocumentsPageComponent implements OnInit {

  constructor(
    private sharedService: SharedService
  ) { }

  ngOnInit(): void {
    this.sharedService.setPageTitle('My Documents');
  }


}

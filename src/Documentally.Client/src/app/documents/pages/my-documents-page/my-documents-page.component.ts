import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../shared/shared.service';
import { ActivatedRoute, } from '@angular/router';

@Component({
  selector: 'app-my-documents-page',
  templateUrl: './my-documents-page.component.html',
  styleUrls: ['./my-documents-page.component.css']
})
export class MyDocumentsPageComponent implements OnInit {

  constructor(
    private sharedService: SharedService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    // Subscribe to ActivatedRoute in order to update the Title in the root component
    this.route.params.subscribe(() => {
      this.sharedService.setPageTitle('My Documents');
    });
  }
}

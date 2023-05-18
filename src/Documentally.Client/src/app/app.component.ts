import { Component, OnInit } from '@angular/core';
import { SharedService } from './services/shared.service';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {

  constructor(private sharedService: SharedService) { }

  ngOnInit(): void {
    this.sharedService.setPageTitle('Documentally');
  }

  getPageTitle(): string {
    return this.sharedService.getPageTitle();
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../../shared/shared.service';

@Component({
  selector: 'app-recover-password-page',
  templateUrl: './recover-password-page.component.html',
  styleUrls: ['./recover-password-page.component.css']
})
export class RecoverPasswordPageComponent implements OnInit {

  constructor(private route: ActivatedRoute, private sharedService: SharedService) { }

  ngOnInit(): void {
    // Subscribe to ActivatedRoute in order to update the Title in the root component
    this.route.params.subscribe(() => {
      this.sharedService.setPageTitle('Recover Password');
    });
  }

}

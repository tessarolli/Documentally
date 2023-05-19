import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../../shared/shared.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {

  constructor(private route: ActivatedRoute, private sharedService: SharedService) { }

  ngOnInit(): void {
    // Subscribe to ActivatedRoute in order to update the Title in the root component
    this.route.params.subscribe(() => {
      this.sharedService.setPageTitle('Register');
    });
  }
}

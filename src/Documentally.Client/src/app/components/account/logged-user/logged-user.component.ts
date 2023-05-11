import { Component } from '@angular/core';

@Component({
  selector: 'app-logged-user',
  templateUrl: './logged-user.component.html',
  styleUrls: ['./logged-user.component.css'],
})
export class LoggedUserComponent {
  overlayVisible: boolean = false;

  toggle() {
        this.overlayVisible = !this.overlayVisible;
    }
}

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private pageTitle: string = '';

  setPageTitle(value: string) {
    this.pageTitle = value;
  }

  getPageTitle() {
    return this.pageTitle;
  }
}

import { Injectable } from '@angular/core';
import { AlertController } from '@ionic/angular';

@Injectable({
  providedIn: 'root'
})
export class AlertService {


  constructor(
    private alertController: AlertController,
  ) { }

  // Method for displaying an error message for the user
  async Error(message: string) {
    await this.Message(message, 'Error');
  }

  // Method for displaying an message for the user
  async Message(message: string, title = 'Message') {
    const alert = await this.alertController.create({
      header: title,
      message: `${message}`.replace(/\n/g, '<br/>'),
      buttons: ['OK'],
    });

    await alert.present();
  }

  // Method for displaying a confirmation message
  async Confirmation(message: string, callback: () => void, title = 'Confirm Operation') {
    const alert = await this.alertController.create({
      header: title,
      message: `${message}`.replace(/\n/g, '<br/>'),
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
        },
        {
          text: 'Confirm',
          handler: callback,
        },
      ],
    });

    await alert.present();
  }
}

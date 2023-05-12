import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { delay } from 'rxjs/operators';
import { AuthenticatedUser } from './models/authenticatedUser.model';

@Injectable()
export class AuthenticationService {

  // Simulating login API call
  Login(email: string, password: string): Observable<AuthenticatedUser> {
    if (email === 'user@documentally.com' && password === 'user') {
      const user: AuthenticatedUser = { id: 1, email: 'user@documentally.com', firstName: 'User', lastName: 'User', token: 'abcd_token' };
      return of(user).pipe(delay(1000)); // Simulating delay for API call
    }
    
    // Return an error if login credentials are invalid
    return throwError(() => new Error('Invalid credentials'));
  }

}

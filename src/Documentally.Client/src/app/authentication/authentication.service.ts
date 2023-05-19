import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, delay, tap } from 'rxjs/operators';
import { AuthenticatedUser } from './models/authenticatedUser.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  })
};

@Injectable()
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  Login(email: string, password: string): Observable<AuthenticatedUser> {
    return this.http.post<AuthenticatedUser>('https://localhost:5001/authentication/login', { email, password }, httpOptions)
      .pipe(
        catchError((error: any) => {
          return throwError(() => new Error(error.error.errors));
        })
      );
  }

}

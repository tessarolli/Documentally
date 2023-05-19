import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthenticatedUser } from './models/authenticatedUser.model';
import { HttpHeaders } from '@angular/common/http';
import { ApiService } from '../core/api.service';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  })
};

@Injectable()
export class AuthenticationService {
  path: string = 'authentication';

  constructor(private apiService: ApiService) { }

  Login(email: string, password: string): Observable<AuthenticatedUser> {
    return this.apiService.post(`/${this.path}/login`, { email, password });
  }

  Register(firstName: string, lastName: string, email: string, password: string): Observable<AuthenticatedUser> {
    return this.apiService.post(`/${this.path}/register`, { firstName, lastName, email, password });
  }
}

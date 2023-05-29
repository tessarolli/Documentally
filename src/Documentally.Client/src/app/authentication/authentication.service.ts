import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticatedUser } from './models/authenticatedUser.model';
import { ApiService } from '../core/services/api.service';

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

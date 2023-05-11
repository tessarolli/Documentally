import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  apiUrl: string = 'https://localhost:5001';

  constructor(private http: HttpClient) { }

  login(username: string, password: string): void {
    const loginRequest = { username, password };
    this.http.post(`${this.apiUrl}/login`, loginRequest).subscribe(
      response => {
        const token: any = response; 
        localStorage.setItem('token', token);
        console.log('Login successful:', response);
      },
      error => {
        console.error('Login failed:', error);
      }
    );
  }
}

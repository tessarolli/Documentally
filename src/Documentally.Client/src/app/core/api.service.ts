import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpHeaders, HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(
    private http: HttpClient
  ) { }

  private formatErrors(error: any): Observable<never> {
    if (error.error && error.error.errors) {
        // API returned user-friendly error messages
        const userFriendlyErrors = error.error.errors;
        return throwError(() => new Error(userFriendlyErrors));
    }

    if (error instanceof HttpErrorResponse) {
      // Handle specific HTTP errors
      let errorMessage = '';
      switch (error.status) {
        case 0:
          errorMessage = 'Unable to connect to the server. Please check your internet connection.';
          break;
        case 404:
          errorMessage = 'The specified url was not found on our server.';
          break;
        case 500:
          errorMessage = 'An internal server error occurred. Please try again later.';
          break;
        default:
          errorMessage = 'An unexpected error occurred. Please try again.';
          break;
      }

      if (errorMessage !== '') {
        return throwError(() => new Error(errorMessage));
      }
    } else if (error instanceof Error) {
      // Handle JavaScript runtime errors
      const errorMessage = 'An unexpected error occurred. Please try again.';
      return throwError(() => new Error(errorMessage));
    }

    // Unknown error type
    const errorMessage = 'An unexpected error occurred. Please try again.';
    return throwError(() => new Error(errorMessage));
  }

  get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    return this.http.get(`${environment.apiUrl}${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  put(path: string, body: Object = {}): Observable<any> {
    return this.http.put(
      `${environment.apiUrl}${path}`,
      JSON.stringify(body)
    ).pipe(catchError(this.formatErrors));
  }

  post(path: string, body: Object = {}): Observable<any> {
    return this.http.post(
      `${environment.apiUrl}${path}`,
      JSON.stringify(body)
    ).pipe(catchError(this.formatErrors));
  }

  delete(path: string): Observable<any> {
    return this.http.delete(
      `${environment.apiUrl}${path}`
    ).pipe(catchError(this.formatErrors));
  }
}

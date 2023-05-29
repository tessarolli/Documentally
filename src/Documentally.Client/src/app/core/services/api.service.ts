import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(
    private http: HttpClient
  ) { }

  private formatErrors(error: any): Observable<string> {
    if (error.error && error.error.errors) {
      // API returned user-friendly error messages

      // Create an array to hold the error messages
      const errorMessages: string[] = [];

      // Iterate over each error and add it to the errorMessages array
      for (const key in error.error.errors) {
        if (error.error.errors.hasOwnProperty(key)) {
          errorMessages.push(error.error.errors[key]);
        }
      }

      // Create the final error message to display
      const errorMessage = `One or More errors Occurred:\n${errorMessages.join('\n')}`;

      // Throw an error with the formatted message
      return throwError(() => new Error(errorMessage));
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
    ).pipe(
      catchError(this.formatErrors));
  }

  upload(path: string, body: FormData): Observable<any> {
    var headers: HttpHeaders = new HttpHeaders();
    headers = headers.set('Content-Type', 'multipart/form-data');

    return this.http.post(
      `${environment.apiUrl}${path}`,
      body, {
      headers: headers,
      reportProgress: true,
      observe: 'events'
    }
    ).pipe(
      catchError(this.formatErrors));
  }

  delete(path: string): Observable<any> {
    return this.http.delete(
      `${environment.apiUrl}${path}`
    ).pipe(catchError(this.formatErrors));
  }
}

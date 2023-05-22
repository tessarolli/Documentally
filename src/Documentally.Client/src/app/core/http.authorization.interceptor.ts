import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, switchMap, take } from 'rxjs';
import { AppState } from '../app.state';
import { Store } from '@ngrx/store';
import { selectAuthenticatedUser } from '../authentication/state/authentication.selectors';

@Injectable()
export class HttpAuthorizationInterceptor implements HttpInterceptor {

  constructor(private store: Store<AppState>) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.store.select(selectAuthenticatedUser).pipe(
      take(1),
      switchMap(authenticatedUser => {

        if (authenticatedUser && authenticatedUser.token) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${authenticatedUser.token}`
            }
          });
        }

        request = request.clone({
          setHeaders: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
          }
        });

        return next.handle(request);
      })
    );
  }
}
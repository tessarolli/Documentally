import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, switchMap, take } from 'rxjs';
import { AppState } from '../app.state';
import { Store } from '@ngrx/store';
import { selectAuthenticatedUser } from '../authentication/state/authentication.selectors';

@Injectable()
export class HttpAuthorizationInterceptor implements HttpInterceptor {

  constructor(private store: Store<AppState>) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    return this.store.select(selectAuthenticatedUser).pipe(
      take(1),
      switchMap(authenticatedUser => {

        request = request.clone({
          setHeaders: {
            'Accept': 'application/json'
          }
        });

        if (authenticatedUser?.token) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${authenticatedUser.token}`
            }
          });
        }

        if (!(request.body instanceof FormData)) {
          request = request.clone({
            setHeaders: {
              'Content-Type': 'application/json',
            }
          });
        }

        return next.handle(request);
      })
    );
  }
}

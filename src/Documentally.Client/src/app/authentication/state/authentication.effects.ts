import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { tap, catchError, map, exhaustMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';
import { Login, SetAuthenticatedUser } from './authentication.actions';

@Injectable()
export class AuthenticationEffects {
  constructor(
    private actions$: Actions,
    private router: Router,
    private authService: AuthenticationService
  ) { }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(Login),
      exhaustMap((action) =>
        this.authService.Login(action.email, action.password).pipe(
          map((authenticatedUser) => SetAuthenticatedUser({ authenticatedUser: authenticatedUser })),
          catchError((error) => {
            // Handle login error, display error message, etc.
            console.error('Login error:', error);
            return of(SetAuthenticatedUser({ authenticatedUser: null }));
          })
        )
      ),
      tap(({ authenticatedUser }) => {
        if (authenticatedUser) {
          // Redirect to a specific route after successful login
          this.router.navigate(['/dashboard']);
        } else {
          // Redirect to the login page or display an error message
          this.router.navigate(['/login']);
        }
      })
    )
  );
}

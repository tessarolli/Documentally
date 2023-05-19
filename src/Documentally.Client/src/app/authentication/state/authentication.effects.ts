import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AuthenticationService } from '../authentication.service';
import { Login, LoginFailure, LoginSuccess, Logout, Register } from './authentication.actions';

@Injectable()
export class AuthenticationEffects {
  constructor(
    private actions$: Actions,
    private authService: AuthenticationService
  ) { }

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(Login),
      mergeMap((action) =>
        this.authService.Login(action.email, action.password).pipe(
          map((authenticatedUser) => LoginSuccess({ authenticatedUser: authenticatedUser })),
          catchError(async (error) => LoginFailure({ error: error.message })
          )
        )
      )
    )
  );

  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(Register),
      mergeMap((action) =>
        this.authService.Register(action.firstName, action.lastName, action.email, action.password).pipe(
          map((authenticatedUser) => LoginSuccess({ authenticatedUser: authenticatedUser })),
          catchError(async (error) => LoginFailure({ error: error.message })
          )
        )
      )
    )
  );

}

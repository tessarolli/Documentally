import { createAction, props } from '@ngrx/store';
import { AuthenticatedUser } from '../models/authenticatedUser.model';

export enum AuthenticationActionTypes {
  Login = '[Authentication] Login',
  LoginSuccess = '[Authentication] Login Success',
  LoginFailure = '[Authentication] Login Failure',
  Logout = '[Authentication] Logout',
  ClearError = '[Authentication] Clear Error',
  SetIsLoading = '[Authentication] Set isLoading',
}

export const Login = createAction(
  AuthenticationActionTypes.Login,
  props<{ email: string; password: string }>()
);

export const LoginSuccess = createAction(
  AuthenticationActionTypes.LoginSuccess,
  props<{ authenticatedUser: AuthenticatedUser }>()
);

export const LoginFailure = createAction(
  AuthenticationActionTypes.LoginFailure,
  props<{ error: string }>()
);

export const Logout = createAction(AuthenticationActionTypes.Logout);

export const ClearError = createAction(AuthenticationActionTypes.ClearError);

export const SetIsLoading = createAction(
  AuthenticationActionTypes.SetIsLoading,
  props<{ isLoading: boolean }>()
);

import { createReducer, on } from '@ngrx/store';
import { initialAuthenticationState } from './authentication.state';
import { Login, LoginSuccess, LoginFailure, Logout, SetAuthenticatedUser } from './authentication.actions';

export const AuthenticationReducer = createReducer(
  initialAuthenticationState,

  on(Login, (state) => ({ ...state, isLoading: true })),

  on(LoginSuccess, (state, { authenticatedUser }) => ({
    ...state,
    authenticatedUser: authenticatedUser,
    authenticated: true,
    isLoading: false,
    error: ''
  })),

  on(LoginFailure, (state, { error }) => ({
    ...state,
    authenticatedUser: null,
    authenticated: false,
    isLoading: false,
    error: error
  })),

  on(Logout, () => {
    console.log('logout');

    return initialAuthenticationState;
  }),

  on(SetAuthenticatedUser, (state, { authenticatedUser }) => ({
    ...state,
    authenticatedUser: authenticatedUser,
    authenticated: !!authenticatedUser,
    isLoading: false,
    error: ''
  }))
);

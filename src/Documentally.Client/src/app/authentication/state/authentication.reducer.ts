import { createReducer, on } from '@ngrx/store';
import { initialAuthenticationState } from './authentication.state';
import { Login, LoginSuccess, LoginFailure, Logout, SetIsLoading, ClearError } from './authentication.actions';

export const AuthenticationReducer = createReducer(
  initialAuthenticationState,

  on(Login, (state) => ({ ...state, isLoading: true })),

  on(LoginSuccess, (state, { authenticatedUser }) => ({
    ...state,
    authenticatedUser: authenticatedUser,
    isAuthenticated: true,
    isLoading: false,
    error: ''
  })),

  on(LoginFailure, (state, { error }) => ({
    ...initialAuthenticationState,
    authenticatedUser: null,
    isAuthenticated: false,
    isLoading: false,
    error: error
  })),

  on(Logout, () => {
    console.log('logout');

    return initialAuthenticationState;
  }),

  on(SetIsLoading, (state, { isLoading }) => ({
    ...state,
    isLoading,
  })),

  on(ClearError, (state) => ({
    ...state,
    error: '',
  }))
);

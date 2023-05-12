import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthenticationState } from './authentication.state';

// Select the authentication feature state
export const selectAuthenticationState = createFeatureSelector<AuthenticationState>('authentication');

// Select the authenticated user
export const selectAuthenticatedUser = createSelector(
  selectAuthenticationState,
  (state) => state.authenticatedUser
);

// Select the authentication status
export const selectIsAuthenticated = createSelector(
  selectAuthenticationState,
  (state) => state.isAuthenticated
);

// Select the loading status
export const selectIsLoading = createSelector(
  selectAuthenticationState,
  (state) => state.isLoading
);

// Select the error
export const selectError = createSelector(
  selectAuthenticationState,
  (state) => state.error
);

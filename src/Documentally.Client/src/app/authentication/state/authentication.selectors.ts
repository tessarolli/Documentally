import { AppState } from '../../app.state';

// Select the authentication feature state
export const selectAuthenticationState = (state: AppState) => state?.authentication;

// Select the authenticated user
export const selectAuthenticatedUser = (state: AppState) => state?.authentication?.authenticatedUser;

// Select the authentication status
export const selectIsAuthenticated = (state: AppState) => state?.authentication?.isAuthenticated;

// Select the loading status
export const selectIsLoading = (state: AppState) => state?.authentication?.isLoading;

// Select the error
export const selectError = (state: AppState) => state?.authentication?.error;

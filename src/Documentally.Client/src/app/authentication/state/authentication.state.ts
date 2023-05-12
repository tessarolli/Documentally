import { AuthenticatedUser } from "../models/authenticatedUser.model";

export interface AuthenticationState {
  authenticatedUser: AuthenticatedUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string;
}

export const initialAuthenticationState: AuthenticationState = {
  authenticatedUser: null,
  isAuthenticated: false,
  isLoading: false,
  error: '',
};

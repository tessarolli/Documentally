import { AuthenticationState } from './authentication/state/authentication.state';

export interface AppState {
  authentication: AuthenticationState;
}

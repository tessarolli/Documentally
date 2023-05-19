import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { AuthenticationState } from './authentication/state/authentication.state';
import { AuthenticationReducer } from './authentication/state/authentication.reducer';
import { hydrationMetaReducer } from './core/hydration.meta-reducer';

export interface AppState {
  authentication: AuthenticationState;
}

export const reducers: ActionReducerMap<AppState> = {
  authentication: AuthenticationReducer
}

export const metaReducers: MetaReducer[] = [
  hydrationMetaReducer
]

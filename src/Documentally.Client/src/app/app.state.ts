import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { AuthenticationState } from './authentication/state/authentication.state';
import { AuthenticationReducer } from './authentication/state/authentication.reducer';
import { hydrationMetaReducer } from './core/hydration.meta-reducer';
import { RootState } from './core/state/root.state';
import { RootReducer } from './core/state/root.reducer';

export interface AppState {
  authentication: AuthenticationState;
  root: RootState;
}

export const reducers: ActionReducerMap<AppState> = {
  authentication: AuthenticationReducer,
  root: RootReducer
}

export const metaReducers: MetaReducer[] = [
  hydrationMetaReducer
]

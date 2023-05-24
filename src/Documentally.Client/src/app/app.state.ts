import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { AuthenticationState } from './authentication/state/authentication.state';
import { AuthenticationReducer } from './authentication/state/authentication.reducer';
import { hydrationMetaReducer } from './core/hydration.meta-reducer';
import { RootState } from './core/state/root.state';
import { RootReducer } from './core/state/root.reducer';
import { MyDocumentsState } from './documents/pages/my-documents-page/my-documents-page.state';
import { MyDocumentsPageReducer } from './documents/pages/my-documents-page/my-documents-page.reducer';

export interface AppState {
  authentication: AuthenticationState;
  root: RootState;
  myDocuments: MyDocumentsState;
}

export const reducers: ActionReducerMap<AppState> = {
  authentication: AuthenticationReducer,
  root: RootReducer,
  myDocuments: MyDocumentsPageReducer,
}

export const metaReducers: MetaReducer[] = [
  hydrationMetaReducer
]

import { createReducer, on } from '@ngrx/store';
import { SetTitle } from './root.actions';
import { initialRootState } from './root.state';

export const RootReducer = createReducer(
  initialRootState,

  on(SetTitle, (state, { title } ) => ({
    ...state,
    pageTitle: title
  })),

);

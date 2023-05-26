import { createReducer, on } from '@ngrx/store';
import { initialSharedWithMeState } from './shared-with-me-page.state';
import * as SharedWithMeActions from './shared-with-me-page.actions';

export const SharedWithMePageReducer = createReducer(
  initialSharedWithMeState,

  on(SharedWithMeActions.LoadSharedWithMe, (state) => ({
    ...state,
    loading: true,
    error: '',
  })),

  on(SharedWithMeActions.LoadSharedWithMeSuccess, (state, { documents }) => ({
    ...state,
    documents,
    loading: false,
  })),

  on(SharedWithMeActions.LoadSharedWithMeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(SharedWithMeActions.ClearError, state => ({
    ...state,
    error: ''
  })),


  on(SharedWithMeActions.SetIsLoading, (state, { isLoading }) => ({
    ...state,
    isLoading
  }))

);

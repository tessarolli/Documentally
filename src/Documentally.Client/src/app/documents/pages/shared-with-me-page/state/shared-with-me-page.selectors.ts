import { createSelector } from '@ngrx/store';
import { AppState } from '../../../../app.state';
import { SharedWithMeState } from './shared-with-me-page.state';

export const selectSharedWithMeState = (state: AppState) => state.myDocuments;

export const selectSharedWithMe = createSelector(
  selectSharedWithMeState,
  (state: SharedWithMeState) => state.documents
);

export const selectSharedWithMeLoading = createSelector(
  selectSharedWithMeState,
  (state: SharedWithMeState) => state.isLoading
);

export const selectSharedWithMeError = createSelector(
  selectSharedWithMeState,
  (state: SharedWithMeState) => state.error
);

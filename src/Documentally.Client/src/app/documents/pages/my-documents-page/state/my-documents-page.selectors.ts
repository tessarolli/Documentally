import { createSelector } from '@ngrx/store';
import { AppState } from '../../../../app.state';
import { MyDocumentsState } from './my-documents-page.state';

export const selectMyDocumentsState = (state: AppState) => state.myDocuments;

export const selectMyDocuments = createSelector(
  selectMyDocumentsState,
  (state: MyDocumentsState) => state.documents
);

export const selectMyDocumentsLoading = createSelector(
  selectMyDocumentsState,
  (state: MyDocumentsState) => state.isLoading
);

export const selectMyDocumentsError = createSelector(
  selectMyDocumentsState,
  (state: MyDocumentsState) => state.error
);

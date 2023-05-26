import { createAction, props } from '@ngrx/store';
import { DocumentModel } from '../../../models/document.model';

export enum SharedWithMeActionTypes {
  LoadSharedWithMe = '[Shared Documents] Load Shared Documents',
  LoadSuccess = '[Shared Documents] Load Shared Documents Success',
  LoadFailure = '[Shared Documents] Load Shared Documents Failure',
  ClearError = '[Shared Documents] Clear Error',
  SetIsLoading = '[Shared Documents] Set isLoading',
}

export const LoadSharedWithMe = createAction(SharedWithMeActionTypes.LoadSharedWithMe);

export const LoadSharedWithMeSuccess = createAction(
  SharedWithMeActionTypes.LoadSuccess,
  props<{ documents: DocumentModel[] }>()
);

export const LoadSharedWithMeFailure = createAction(
  SharedWithMeActionTypes.LoadFailure,
  props<{ error: string }>()
);

export const ClearError = createAction(SharedWithMeActionTypes.ClearError);

export const SetIsLoading = createAction(
  SharedWithMeActionTypes.SetIsLoading,
  props<{ isLoading: boolean }>()
);


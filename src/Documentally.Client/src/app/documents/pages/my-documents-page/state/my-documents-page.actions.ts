import { createAction, props } from '@ngrx/store';
import { DocumentModel } from '../../../models/document.model';

export enum MyDocumentsActionTypes {
  LoadMyDocuments = '[My Documents] Load My Documents',
  LoadSuccess = '[My Documents] Load My Documents Success',
  LoadFailure = '[My Documents] Load My Documents Failure',
  ClearError = '[My Documents] Clear Error',
  SetIsLoading = '[My Documents] Set isLoading',
}

export const LoadMyDocuments = createAction(MyDocumentsActionTypes.LoadMyDocuments);

export const LoadMyDocumentsSuccess = createAction(
  MyDocumentsActionTypes.LoadSuccess,
  props<{ documents: DocumentModel[] }>()
);

export const LoadMyDocumentsFailure = createAction(
  MyDocumentsActionTypes.LoadFailure,
  props<{ error: string }>()
);

export const ClearError = createAction(MyDocumentsActionTypes.ClearError);

export const SetIsLoading = createAction(
  MyDocumentsActionTypes.SetIsLoading,
  props<{ isLoading: boolean }>()
);


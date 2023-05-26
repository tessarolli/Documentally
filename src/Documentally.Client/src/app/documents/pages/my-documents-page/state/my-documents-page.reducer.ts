import { createReducer, on } from '@ngrx/store';
import { initialMyDocumentsState } from './my-documents-page.state';
import * as MyDocumentsActions from './my-documents-page.actions';

export const MyDocumentsPageReducer = createReducer(
  initialMyDocumentsState,

  on(MyDocumentsActions.LoadMyDocuments, (state) => ({
    ...state,
    loading: true,
    error: '',
  })),

  on(MyDocumentsActions.LoadMyDocumentsSuccess, (state, { documents }) => ({
    ...state,
    documents,
    loading: false,
  })),

  on(MyDocumentsActions.LoadMyDocumentsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(MyDocumentsActions.ClearError, state => ({
    ...state,
    error: ''
  })),


  on(MyDocumentsActions.SetIsLoading, (state, { isLoading }) => ({
    ...state,
    isLoading
  }))

);

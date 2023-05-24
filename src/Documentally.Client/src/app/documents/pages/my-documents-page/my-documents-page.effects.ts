import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap, withLatestFrom } from 'rxjs/operators';
import { LoadMyDocuments, LoadMyDocumentsFailure, LoadMyDocumentsSuccess } from './my-documents-page.actions';
import { DocumentsService } from '../../services/documents.service';
import { Action, Store } from '@ngrx/store';
import { DocumentModel } from '../../models/document.model';
import { AppState } from '../../../app.state';
import { selectAuthenticatedUser } from '../../../authentication/state/authentication.selectors';

@Injectable()
export class MyDocumentsPageEffects {

  constructor(
    private actions$: Actions,
    private documentsService: DocumentsService,
    private store: Store<AppState>
  ) { }


  loadMyDocuments$: Observable<Action> = this.actions$.pipe(
    ofType(LoadMyDocuments),
    withLatestFrom(this.store.select(selectAuthenticatedUser)),
    switchMap(([action, authenticatedUser]) =>
      this.documentsService.GetUserDocuments(authenticatedUser?.id).pipe(
        map((documents: DocumentModel[]) =>
          LoadMyDocumentsSuccess({ documents })
        ),
        catchError(async (error: Error) => LoadMyDocumentsFailure({ error: error.message })
        )
      )
    )
  );


}

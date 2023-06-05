import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { LoadMyDocuments, LoadMyDocumentsFailure, LoadMyDocumentsSuccess } from './my-documents-page.actions';
import { DocumentsService } from '../../../services/documents.service';
import { Action, Store } from '@ngrx/store';
import { DocumentModel } from '../../../models/document.model';
import { AppState } from '../../../../app.state';
import { selectAuthenticatedUser } from '../../../../authentication/state/authentication.selectors';

@Injectable()
export class MyDocumentsPageEffects {

  constructor(
    private actions$: Actions,
    private documentsService: DocumentsService,
    private store: Store<AppState>
  ) { }


  loadMyDocuments$: Observable<Action> = createEffect(() =>
    this.actions$.pipe(
      ofType(LoadMyDocuments),
      switchMap(() =>
        this.store.select(selectAuthenticatedUser).pipe(
          switchMap((authenticatedUser) => {
            if (authenticatedUser) {
              return this.documentsService.GetUserDocuments(authenticatedUser.id).pipe(
                map((documents: DocumentModel[]) => LoadMyDocumentsSuccess({ documents })),
                catchError((error: Error) => of(LoadMyDocumentsFailure({ error: error.message })))
              );
            } else {
              // Handle the case when the user is not authenticated
              return of(LoadMyDocumentsFailure({ error: 'User is not authenticated.' }));
            }
          })
        )
      )
    )
  );


}

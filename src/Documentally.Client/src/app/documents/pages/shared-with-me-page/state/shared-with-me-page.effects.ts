import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { LoadSharedWithMe, LoadSharedWithMeFailure, LoadSharedWithMeSuccess } from './shared-with-me-page.actions';
import { DocumentsService } from '../../../services/documents.service';
import { Action, Store } from '@ngrx/store';
import { DocumentModel } from '../../../models/document.model';
import { AppState } from '../../../../app.state';
import { selectAuthenticatedUser } from '../../../../authentication/state/authentication.selectors';

@Injectable()
export class SharedWithMePageEffects {

  constructor(
    private actions$: Actions,
    private documentsService: DocumentsService,
    private store: Store<AppState>
  ) { }


  loadSharedWithMe$: Observable<Action> = createEffect(() =>
    this.actions$.pipe(
      ofType(LoadSharedWithMe),
      switchMap(() =>
        this.store.select(selectAuthenticatedUser).pipe(
          switchMap((authenticatedUser) => {
            if (authenticatedUser) {
              return this.documentsService.GetDocumentsSharedWithUser(authenticatedUser.id).pipe(
                map((documents: DocumentModel[]) => LoadSharedWithMeSuccess({ documents })),
                catchError((error: Error) => of(LoadSharedWithMeFailure({ error: error.message })))
              );
            } else {
              // Handle the case when the user is not authenticated
              return of(LoadSharedWithMeFailure({ error: 'User is not authenticated.' }));
            }
          })
        )
      )
    )
  );


}

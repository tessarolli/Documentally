import { createAction, props } from '@ngrx/store';

export enum RootActionTypes {
  SetTitle = '[RootState] Set Title',
}

export const SetTitle = createAction(
  RootActionTypes.SetTitle,
  props<{ title: string; }>()
);

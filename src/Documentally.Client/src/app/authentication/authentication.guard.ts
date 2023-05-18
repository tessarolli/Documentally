import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { selectIsAuthenticated } from './state/authentication.selectors';

export const authenticationGuard = () => {
  const store = inject(Store);
  const router = inject(Router);

  return store.pipe(
    select(selectIsAuthenticated),
    map((isAuthenticated: boolean) => {

      if (isAuthenticated) {
        return true;
      }

      return router.navigate(['/login']);
    })
  );
};

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authenticationGuard } from '../authentication/authentication.guard';
import { MyDocumentsPageComponent } from './pages/my-documents-page/my-documents-page.component';
import { SharedWithMePageComponent } from './pages/shared-with-me-page/shared-with-me-page.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        redirectTo: 'mydocuments',
        pathMatch: 'full'
      },
      {
        path: 'mydocuments',
        canActivate: [authenticationGuard],
        component: MyDocumentsPageComponent,
      },
      {
        path: 'shared',
        canActivate: [authenticationGuard],
        component: SharedWithMePageComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentsRoutingModule { }

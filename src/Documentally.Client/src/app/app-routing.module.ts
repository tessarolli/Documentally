import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'documents',
    pathMatch: 'full'
  },
  {
    path: 'documents',
    loadChildren: () => import('./documents/documents.module').then(m => m.DocumentsModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule,
  ]
})
export class AppRoutingModule { }

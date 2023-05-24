import { Injectable } from '@angular/core';
import { ApiService } from '../../core/api.service';
import { Observable } from 'rxjs';
import { DocumentModel } from '../models/document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentsService {
  path: string = 'documents';

  constructor(private apiService: ApiService) { }

  GetUserDocuments(userId: number | undefined): Observable<DocumentModel[]> {
    return this.apiService.get(`/${this.path}/getuserdocuments/${userId}`);
  }

  GetDocumentsSharedWithUser(userId: number | undefined): Observable<DocumentModel[]> {
    return this.apiService.get(`/${this.path}/getdocumentssharedwithuser/${userId}`);
  }

  GetDocumentById(documentId: number | undefined): Observable<DocumentModel> {
    return this.apiService.get(`/${this.path}/getdocumentbyid/${documentId}`);
  }

}

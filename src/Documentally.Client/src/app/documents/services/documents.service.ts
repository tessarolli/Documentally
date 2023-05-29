import { Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { Observable } from 'rxjs';
import { DocumentModel } from '../models/document.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentsService {

  path: string = 'documents';

  constructor(private apiService: ApiService) { }

  GetUserDocuments(userId: number | undefined): Observable<DocumentModel[]> {
    return this.apiService.get(`/${this.path}/user/${userId}`);
  }

  GetDocumentsSharedWithUser(userId: number | undefined): Observable<DocumentModel[]> {
    return this.apiService.get(`/${this.path}/sharedwithuser/${userId}`);
  }

  GetDocumentById(documentId: number | undefined): Observable<DocumentModel> {
    return this.apiService.get(`/${this.path}/${documentId}`);
  }

  UploadDocument(formData: FormData) {
    return this.apiService.upload(`/${this.path}`, formData);
  }
}

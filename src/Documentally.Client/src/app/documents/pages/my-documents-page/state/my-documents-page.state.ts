import { DocumentModel } from "../../../models/document.model";

export interface MyDocumentsState {
  documents: DocumentModel[];
  isLoading: boolean;
  error: string;
}

export const initialMyDocumentsState: MyDocumentsState = {
  documents: [],
  isLoading: false,
  error: '',
};

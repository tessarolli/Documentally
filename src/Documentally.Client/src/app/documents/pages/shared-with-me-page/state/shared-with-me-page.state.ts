import { DocumentModel } from "../../../models/document.model";

export interface SharedWithMeState {
  documents: DocumentModel[];
  isLoading: boolean;
  error: string;
}

export const initialSharedWithMeState: SharedWithMeState = {
  documents: [],
  isLoading: false,
  error: '',
};

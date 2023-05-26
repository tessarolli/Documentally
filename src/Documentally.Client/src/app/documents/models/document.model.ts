export interface DocumentModel {
  id: number;
  ownerId: number;
  name: string;
  description?: string;
  category?: string;
  size: number;
  blobUrl: string;
  cloudFileName: string;
  sharedGroupIds: number[];
  sharedUserIds: number[];
  postedAtUtc: Date;
}

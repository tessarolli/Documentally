export interface DocumentModel {
  Id: number;
  OwnerId: number;
  Name: string;
  Description?: string;
  Category?: string;
  Size: number;
  BlobUrl: string;
  CloudFileName: string;
  SharedGroupIds: number[];
  SharedUserIds: number[];
  PostedAtUtc: Date;
}

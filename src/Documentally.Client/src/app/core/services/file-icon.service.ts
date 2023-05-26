import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileIconService {

  private fileExtensionMap: { [extension: string]: string } = {
    // Document files
    pdf: 'fas fa-file-pdf',
    doc: 'fas fa-file-word',
    docx: 'fas fa-file-word',
    xls: 'fas fa-file-excel',
    xlsx: 'fas fa-file-excel',

    // Image files
    jpg: 'fas fa-file-image',
    jpeg: 'fas fa-file-image',
    png: 'fas fa-file-image',
    gif: 'fas fa-file-image',
    bmp: 'fas fa-file-image',

    // Video files
    mp4: 'fas fa-file-video',
    mov: 'fas fa-file-video',
    avi: 'fas fa-file-video',
    mkv: 'fas fa-file-video',

    // Audio files
    mp3: 'fas fa-file-audio',
    wav: 'fas fa-file-audio',
    ogg: 'fas fa-file-audio',
    flac: 'fas fa-file-audio',

    // Compressed files
    zip: 'fas fa-file-archive',
    rar: 'fas fa-file-archive',
    gz: 'fas fa-file-archive',

    // Presentation files
    ppt: 'fas fa-file-powerpoint',
    pptx: 'fas fa-file-powerpoint',

    // Code files
    html: 'fas fa-file-code',
    css: 'fas fa-file-code',
    js: 'fas fa-file-code',
    ts: 'fas fa-file-code',

    // Other file types
    txt: 'fas fa-file-alt',
    csv: 'fas fa-file-csv',
    xml: 'fas fa-file-code',
    exe: 'fas fa-file-exe',
    dmg: 'fas fa-file-archive',
  };

  getFileIcon(fileName: string): string {
    const extension = this.getFileExtension(fileName);
    const iconClass = this.fileExtensionMap[extension.toLowerCase()];
    return iconClass ? iconClass : 'fas fa-file'; // Default icon class for unknown extensions
  }

  private getFileExtension(fileName: string): string {
    if (fileName) {
      const parts = fileName.split('.');
      if (parts.length > 1) {
        return parts.pop()!.toLowerCase();
      }
    }
    return '';
  }
}

// <copyright file="IDocumentRepository.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Document;
using FluentResults;

namespace Documentally.Application.Abstractions.Repositories;

/// <summary>
/// Document Repository Interface.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Get a document by its Id.
    /// </summary>
    /// <param name="documentId">The document Id.</param>
    /// <returns>A Result of Document.</returns>
    Task<Result<Document>> GetByIdAsync(DocId documentId);

    /// <summary>
    /// Gets a list of document by its Ids.
    /// </summary>
    /// <param name="documentIds">The array of document Ids.</param>
    /// <returns>A Result of List of Documents.</returns>
    Task<Result<List<Document>>> GetByIdsAsync(DocId[] documentIds);

    /// <summary>
    /// Get all documents from a User Id.
    /// </summary>
    /// <param name="userId">The document Id.</param>
    /// <returns>A Result of List of Documents.</returns>
    Task<Result<List<Document>>> GetAllDocumentsFromUserIdAsync(long userId);

    /// <summary>
    /// Get all documents shared to this User Id (either by direct or group sharing).
    /// </summary>
    /// <param name="userId">The document Id.</param>
    /// <returns>A Result of List of Documents.</returns>
    Task<Result<List<Document>>> GetAllDocumentsSharedToUserIdAsync(long userId);

    /// <summary>
    /// Verify if the User has Access to a Document before downloading.
    /// </summary>
    /// <param name="documentId">The document Id.</param>
    /// <param name="userId">The user Id.</param>
    /// <returns>A Result of Document.</returns>
    Task<Result> VerifyUserIdHasAccessToDocIdAsync(DocId documentId, long userId);

    /// <summary>
    /// Adds a document to the repository.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <returns>A Result of Document.</returns>
    Task<Result<Document>> AddAsync(Document document);

    /// <summary>
    /// Updates a document in the repository.
    /// </summary>
    /// <param name="document">The document Id.</param>
    /// <returns>A Result of Document.</returns>
    Task<Result<Document>> UpdateAsync(Document document);

    /// <summary>
    /// Removes a document from the repository.
    /// </summary>
    /// <param name="documentId">The document Id.</param>
    /// <returns>A Result of the operation.</returns>
    Task<Result> RemoveAsync(DocId documentId);

    /// <summary>
    /// Shares a document with a Group.
    /// </summary>
    /// <param name="documentId">The document Id.</param>
    /// <param name="groupId">The Group Id.</param>
    /// <returns>A Result of the operation.</returns>
    Task<Result> ShareDocumentWithGroupAsync(DocId documentId, long groupId);

    /// <summary>
    /// Shares a document with a User.
    /// </summary>
    /// <param name="documentId">The document Id.</param>
    /// <param name="userId">The User Id.</param>
    /// <returns>A Result of the operation.</returns>
    Task<Result> ShareDocumentWithUserAsync(DocId documentId, long userId);

    /// <summary>
    /// Removes this document from the shared group.
    /// </summary>
    /// <param name="documentId">The document Id to remove.</param>
    /// <param name="groupId">The group Id to remove.</param>
    /// <returns>The Result of the operation.</returns>
    Task<Result> RemoveDocumentShareFromGroupAsync(DocId documentId, long groupId);

    /// <summary>
    /// Removes this document from the shared user.
    /// </summary>
    /// <param name="documentId">The document Id to remove.</param>
    /// <param name="userId">The user Id to remove.</param>
    /// <returns>The Result of the operation.</returns>
    Task<Result> RemoveDocumentShareFromUserAsync(DocId documentId, long userId);
}

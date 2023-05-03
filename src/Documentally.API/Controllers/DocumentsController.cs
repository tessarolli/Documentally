// <copyright file="DocumentsController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Abstractions.Services;
using Documentally.Application.Documents.Commands.ShareDocumentWithGroup;
using Documentally.Application.Documents.Commands.ShareDocumentWithUser;
using Documentally.Application.Documents.Commands.UnshareDocumentWithGroup;
using Documentally.Application.Documents.Commands.UnshareDocumentWithUser;
using Documentally.Application.Documents.Commands.UpdateDocument;
using Documentally.Application.Documents.Queries.DownloadDocumentQuery;
using Documentally.Application.Documents.Queries.GetDocumentById;
using Documentally.Application.Documents.Results;
using Documentally.Application.UserDocuments.Queries.GetUserDocumentssList;
using Documentally.Application.Users.Commands.DeleteDocument;
using Documentally.Application.Users.Commands.UploadDocument;
using Documentally.Contract.Documents.Requests;
using Documentally.Contracts.Document.Requests;
using Documentally.Contracts.Document.Responses;
using Documentally.Domain.Enums;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Documents Controller.
/// </summary>
[Route("[controller]")]
public class DocumentsController : ResultControllerBase<DocumentsController>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentsController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    /// <param name="exceptionHandlingService">Injected exceptionHandlingService.</param>
    public DocumentsController(IMediator mediator, IMapper mapper, ILogger<DocumentsController> logger, IExceptionHandlingService exceptionHandlingService)
        : base(mediator, mapper, logger, exceptionHandlingService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    /// <summary>
    /// Downloads a Document by its Id.
    /// </summary>
    /// <param name="id">Document Id.</param>
    /// <returns>The Document Aggregate.</returns>
    [HttpGet("{id:long}/download")]
    [RoleAuthorize]
    public async Task<IActionResult> DownloadDocument(long id)
    {
        logger.LogInformation("GET /Documents/Id/Download called");

        var result = await mediator.Send(new DownloadDocumentQuery(id));

        return ValidateResult(
            result,
            () => File(result.Value.Item1, result.Value.Item2),
            () => Problem());
    }

    /// <summary>
    /// Updload a Document to the Cloud Storage Provider and Persist it in the Document Repository.
    /// </summary>
    /// <param name="request">Document data.</param>
    /// <returns>The Document instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(new[] { Roles.Admin, Roles.Manager })]
    public async Task<ActionResult<DocumentResponse>> UploadDocument([FromForm] UploadDocumentRequest request)
    {
        logger.LogInformation("POST /Documents called");

        var command = new UploadDocumentCommand(
            request.FileName,
            request.Description,
            request.Category,
            request.File);

        var result = await mediator.Send(command);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<DocumentResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Delete a Document from the Cloud Storage Provider and Document Repository.
    /// </summary>
    /// <param name="request">Document Id.</param>
    /// <returns>The Result of the operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> DeleteDocument(DeleteDocumentRequest request) =>
        await HandleRequestAsync<DeleteDocumentCommand, Result, object>(request);

    /// <summary>
    /// Gets a list of Documents that the User Id owns.
    /// </summary>
    /// <param name="request">The User Id.</param>
    /// <returns>The list of Documents.</returns>
    [HttpGet("user/{request:long}")]
    [RoleAuthorize]
    public async Task<IActionResult> GetUserDocuments(long request) =>
        await HandleRequestAsync<GetUserDocumentsListQuery, List<DocumentResult>, List<DocumentResponse>>(request);

    /// <summary>
    /// Gets a list of Documents that the User Id owns.
    /// </summary>
    /// <param name="request">The User Id.</param>
    /// <returns>The list of Documents.</returns>
    [HttpGet("SharedWithUser/{request:long}")]
    [RoleAuthorize]
    public async Task<IActionResult> GetDocumentsSharedWithUser(long request) =>
        await HandleRequestAsync<GetDocumentsSharedWithUserQuery, List<DocumentResult>, List<DocumentResponse>>(request);

    /// <summary>
    /// Gets a Document by its Id.
    /// </summary>
    /// <param name="id">Document Id.</param>
    /// <returns>The Document Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<IActionResult> GetDocumentById(long id) =>
        await HandleRequestAsync<GetDocumentByIdQuery, DocumentResult, DocumentResponse>(id);

    /// <summary>
    /// Updates a Document in the Document Repository.
    /// </summary>
    /// <param name="request">Document data.</param>
    /// <returns>The Document instance created with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> UpdateDocument(UpdateDocumentRequest request) =>
        await HandleRequestAsync<UpdateDocumentCommand, DocumentResult, DocumentResponse>(request);

    /// <summary>
    /// Shares a Document With an User.
    /// </summary>
    /// <param name="request">Document and User Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("ShareWithUser")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> ShareDocumentWithUser(ShareDocumentWithUserRequest request) =>
        await HandleRequestAsync<ShareDocumentWithUserCommand, Result, object>(request);

    /// <summary>
    /// Shares a Document With an Group.
    /// </summary>
    /// <param name="request">Document and Group Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("ShareWithGroup")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> ShareDocumentWithGroup(ShareDocumentWithGroupRequest request) =>
        await HandleRequestAsync<ShareDocumentWithGroupCommand, Result, object>(request);

    /// <summary>
    /// Unshares a Document With an User.
    /// </summary>
    /// <param name="request">Document and User Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("UnshareWithUser")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> UnshareDocumentWithUser(UnshareDocumentWithUserRequest request) =>
        await HandleRequestAsync<UnshareDocumentWithUserCommand, Result, object>(request);

    /// <summary>
    /// Unshares a Document With an Group.
    /// </summary>
    /// <param name="request">Document and Group Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("UnshareWithGroup")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<IActionResult> UnshareDocumentWithGroup(UnshareDocumentWithGroupRequest request) =>
        await HandleRequestAsync<UnshareDocumentWithGroupCommand, Result, object>(request);
}

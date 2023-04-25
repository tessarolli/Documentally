// <copyright file="DocumentsController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Documents.Commands.ShareDocumentWithGroup;
using Documentally.Application.Documents.Commands.ShareDocumentWithUser;
using Documentally.Application.Documents.Commands.UnshareDocumentWithGroup;
using Documentally.Application.Documents.Commands.UnshareDocumentWithUser;
using Documentally.Application.Documents.Commands.UpdateDocument;
using Documentally.Application.Documents.Queries.DownloadDocumentQuery;
using Documentally.Application.Documents.Queries.GetDocumentById;
using Documentally.Application.UserDocuments.Queries.GetUserDocumentssList;
using Documentally.Application.Users.Commands.DeleteDocument;
using Documentally.Application.Users.Commands.UploadDocument;
using Documentally.Contract.Documents.Requests;
using Documentally.Contracts.Document.Requests;
using Documentally.Contracts.Document.Responses;
using Documentally.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Documentally.API.Controllers;

/// <summary>
/// Documents Controller.
/// </summary>
[Route("[controller]")]
public class DocumentsController : ResultControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentsController"/> class.
    /// </summary>
    /// <param name="mediator">Injected mediator.</param>
    /// <param name="mapper">Injected mapper.</param>
    /// <param name="logger">Injected logger.</param>
    public DocumentsController(IMediator mediator, IMapper mapper, ILogger<DocumentsController> logger)
        : base(logger)
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
    public async Task<ActionResult<DocumentResponse>> DownloadDocument(long id)
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
    [RoleAuthorize(Roles.Admin)]
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
    public async Task<ActionResult> DeleteDocument(DeleteDocumentRequest request)
    {
        logger.LogInformation("DELETE /Documents called");

        var command = mapper.Map<DeleteDocumentCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }

    /// <summary>
    /// Gets a list of Documents that the User Id owns.
    /// </summary>
    /// <param name="request">The User Id.</param>
    /// <returns>The list of Documents.</returns>
    [HttpGet("user/{request:long}")]
    [RoleAuthorize]
    public async Task<ActionResult<IEnumerable<DocumentResponse>>> GetUserDocuments(long request)
    {
        logger.LogInformation("GET /Documents/User/{request} called", request);

        var result = await mediator.Send(new GetUserDocumentsListQuery(request));

        return ValidateResult(
            result,
            () => Ok(mapper.Map<List<DocumentResponse>>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Gets a list of Documents that the User Id owns.
    /// </summary>
    /// <param name="request">The User Id.</param>
    /// <returns>The list of Documents.</returns>
    [HttpGet("SharedWithUser/{request:long}")]
    [RoleAuthorize]
    public async Task<ActionResult<IEnumerable<DocumentResponse>>> GetDocumentsSharedWithUser(long request)
    {
        logger.LogInformation("GET /Documents/User/{request} called", request);

        var result = await mediator.Send(new GetDocumentsSharedWithUserQuery(request));

        return ValidateResult(
            result,
            () => Ok(mapper.Map<List<DocumentResponse>>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Gets a Document by its Id.
    /// </summary>
    /// <param name="id">Document Id.</param>
    /// <returns>The Document Aggregate.</returns>
    [HttpGet("{id:long}")]
    [RoleAuthorize]
    public async Task<ActionResult<DocumentResponse>> GetDocumentById(long id)
    {
        logger.LogInformation("GET /Documents/Id called");

        var result = await mediator.Send(new GetDocumentByIdQuery(id));

        return ValidateResult(
            result,
            () => Ok(mapper.Map<DocumentResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Updates a Document in the Document Repository.
    /// </summary>
    /// <param name="request">Document data.</param>
    /// <returns>The Document instance created with Id.</returns>
    [HttpPut]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<DocumentResponse>> UpdateDocument(UpdateDocumentRequest request)
    {
        logger.LogInformation("PUT /Documents called");

        var updateDocumentCommand = mapper.Map<UpdateDocumentCommand>(request);

        var result = await mediator.Send(updateDocumentCommand);

        return ValidateResult(
            result,
            () => Ok(mapper.Map<DocumentResponse>(result.Value)),
            () => Problem());
    }

    /// <summary>
    /// Shares a Document With an User.
    /// </summary>
    /// <param name="request">Document and User Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("ShareWithUser")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> ShareDocumentWithUser(ShareDocumentWithUserRequest request)
    {
        logger.LogInformation("PUT /Documents called");

        var command = mapper.Map<ShareDocumentWithUserCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }

    /// <summary>
    /// Shares a Document With an Group.
    /// </summary>
    /// <param name="request">Document and Group Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("ShareWithGroup")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> ShareDocumentWithGroup(ShareDocumentWithGroupRequest request)
    {
        logger.LogInformation("PUT /Documents called");

        var command = mapper.Map<ShareDocumentWithGroupCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }

    /// <summary>
    /// Unshares a Document With an User.
    /// </summary>
    /// <param name="request">Document and User Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("UnshareWithUser")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> UnshareDocumentWithUser(UnshareDocumentWithUserRequest request)
    {
        logger.LogInformation("PUT /Documents called");

        var command = mapper.Map<UnshareDocumentWithUserCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }

    /// <summary>
    /// Unshares a Document With an Group.
    /// </summary>
    /// <param name="request">Document and Group Id's.</param>
    /// <returns>The status of the operation.</returns>
    [HttpPost("UnshareWithGroup")]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> UnshareDocumentWithGroup(UnshareDocumentWithGroupRequest request)
    {
        logger.LogInformation("PUT /Documents called");

        var command = mapper.Map<UnshareDocumentWithGroupCommand>(request);

        var result = await mediator.Send(command);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }
}

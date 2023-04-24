// <copyright file="DocumentsController.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.API.Common.Attributes;
using Documentally.API.Common.Controllers;
using Documentally.Application.Users.Commands.UploadDocument;
using Documentally.Contract.Documents.Requests;
//using Documentally.Application.Documents.Commands.DeleteDocument;
//using Documentally.Application.Documents.Commands.UpdateDocument;
//using Documentally.Application.Documents.Queries.GetDocumentById;
//using Documentally.Application.Documents.Queries.GetDocumentsList;
//using Documentally.Application.Documents.Results;
//using Documentally.Contracts.Document.Requests;
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
    /// Updload a Document to the Cloud Storage Provider and Persist it in the Document Repository.
    /// </summary>
    /// <param name="request">Document data.</param>
    /// <returns>The Document instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize]
    public async Task<ActionResult<DocumentResponse>> UploadDocument([FromForm] UploadDocumentRequest request)
    {
        logger.LogInformation("POST /Documents called");

        var command = new UploadDocumentCommand(
            request.UserId,
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

    /*
    /// <summary>
    /// Gets a list of Documents.
    /// </summary>
    /// <returns>The list of Documents.</returns>
    [HttpGet]
    [RoleAuthorize]
    public async Task<ActionResult<IEnumerable<DocumentResponse>>> GetDocuments()
    {
        logger.LogInformation("GET /Documents/ called");

        var result = await mediator.Send(new GetDocumentsListQuery());

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
    /// Add a Document to the Document Repository.
    /// </summary>
    /// <param name="request">Document data.</param>
    /// <returns>The Document instance created with Id.</returns>
    [HttpPost]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult<DocumentResponse>> AddDocument(AddDocumentRequest request)
    {
        logger.LogInformation("POST /Documents called");

        var addDocumentCommand = mapper.Map<AddDocumentCommand>(request);

        var result = await mediator.Send(addDocumentCommand);

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
    /// Deletes a Document from the Document Repository.
    /// </summary>
    /// <param name="request">Document Id.</param>
    /// <returns>The Action Result of the delete operation.</returns>
    [HttpDelete]
    [RoleAuthorize(Roles.Admin)]
    public async Task<ActionResult> DeleteDocument(DeleteDocumentRequest request)
    {
        logger.LogInformation("DELETE /Documents called");

        var deleteDocumentCommand = mapper.Map<DeleteDocumentCommand>(request);

        var result = await mediator.Send(deleteDocumentCommand);

        return ValidateResult<object>(
            result,
            () => Ok(),
            () => Problem());
    }

    */
}

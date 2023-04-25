// <copyright file="Document.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.Common.DDD;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Document.Validators;
using Documentally.Domain.User.ValueObjects;
using FluentResults;
using FluentValidation;
using User_ = Documentally.Domain.User.User;

namespace Documentally.Domain.Document;

/// <summary>
/// Document Aggegate.
/// </summary>
public sealed class Document : AggregateRoot<DocId>
{
    /// <summary>
    /// Gets a Empty Document instance.
    /// </summary>
    public static readonly Document Empty = new (new DocId());

    private Document(DocId id, Lazy<User_>? owner = null)
        : base(id)
    {
        Owner = owner ?? new Lazy<User_>(() => User_.Empty);
    }

    /// <summary>
    /// Gets Document's Owner.
    /// </summary>
    public Lazy<User_> Owner { get; private set; }

    /// <summary>
    /// Gets Docment's UserId.
    /// </summary>
    public UserId OwnerId { get; private set; } = null!;

    /// <summary>
    /// Gets Document's Name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets Document's Cloud File Name.
    /// </summary>
    public string CloudFileName { get; private set; } = null!;

    /// <summary>
    /// Gets Document's Description.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets Document's Category.
    /// </summary>
    public string? Category { get; private set; }

    /// <summary>
    /// Gets Document's Size.
    /// </summary>
    public long Size { get; private set; }

    /// <summary>
    /// Gets Document's Blob Url.
    /// </summary>
    public string BlobUrl { get; private set; } = null!;

    /// <summary>
    /// Gets Document's Posted Date on utc.
    /// </summary>
    public DateTime PostedAtUtc { get; private set; }

    /// <summary>
    /// Creates a new Document Entity instance.
    /// </summary>
    /// <param name="id">Document's id.</param>
    /// <param name="ownerId">Document's Owner.</param>
    /// <param name="name">Document's Name.</param>
    /// <param name="description">Document's Description.</param>
    /// <param name="category">Document's Category.</param>
    /// <param name="size">Document's Size.</param>
    /// <param name="blobUrl">Document's Blob Url.</param>
    /// <param name="newFileName">Document's Cloud File Name.</param>
    /// <param name="postedOnUtc">Document's posted date.</param>
    /// <param name="owner">Document's Lazy{User} instance.</param>
    /// <returns>New Document's instance or error.</returns>
    public static Result<Document> Create(
        long? id,
        UserId ownerId,
        string name,
        string? description,
        string? category,
        long size,
        string blobUrl,
        string newFileName,
        DateTime? postedOnUtc = null,
        Lazy<User_>? owner = null)
    {
        var document = new Document(new DocId(id), owner)
        {
            OwnerId = ownerId,
            Name = name,
            Description = description,
            Category = category,
            Size = size,
            BlobUrl = blobUrl,
            CloudFileName = newFileName,
            PostedAtUtc = postedOnUtc ?? DateTime.UtcNow.AddYears(-100),
        };

        var validationResult = document.Validate();
        if (validationResult.IsSuccess)
        {
            return document;
        }
        else
        {
            return Result.Fail(validationResult.Errors);
        }
    }

    /// <inheritdoc/>
    protected override IValidator GetValidator() => new DocumentValidator();
}
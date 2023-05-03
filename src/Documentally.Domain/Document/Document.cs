// <copyright file="Document.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.Abstractions;
using Documentally.Domain.Common.DDD;
using Documentally.Domain.Doc.ValueObjects;
using Documentally.Domain.Document.Validators;
using Documentally.Domain.Group.ValueObjects;
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
    public static readonly Document Empty = new(new DocId());

    private readonly List<UserId> sharedUserIds;
    private readonly List<GroupId> sharedGroupIds;

    private Document(DocId id, Lazy<User_>? owner = null, List<GroupId>? sharedGroupIds = null, List<UserId>? sharedUserIds = null)
        : base(id)
    {
        Owner = owner ?? new Lazy<User_>(() => User_.Empty);
        this.sharedUserIds = sharedUserIds ?? new List<UserId>();
        this.sharedGroupIds = sharedGroupIds ?? new List<GroupId>();
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
    /// Gets the List of Group Ids that this doc is shared with.
    /// </summary>
    public List<GroupId> SharedGroupIds => sharedGroupIds.ToList();

    /// <summary>
    /// Gets the List of User Ids that this doc is shared with.
    /// </summary>
    public List<UserId> SharedUserIds => sharedUserIds.ToList();

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
    /// <param name="sharedGroupIds">Document's list of shared group ids.</param>
    /// <param name="sharedUserIds">Document's list of shared user ids.</param>
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
        Lazy<User_>? owner = null,
        List<GroupId>? sharedGroupIds = null,
        List<UserId>? sharedUserIds = null)
    {
        var document = new Document(new DocId(id), owner, sharedGroupIds, sharedUserIds)
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

    /// <summary>
    /// Shares this document with an User.
    /// </summary>
    /// <param name="userId">The User Id.</param>
    /// <returns>The result of the operation.</returns>
    public Result ShareWithUser(UserId userId)
    {
        if (sharedUserIds.Any(x => x.Equals(userId)))
        {
            return Result.Fail($"This Document is already shared with this User({userId.Value})");
        }

        sharedUserIds.Add(userId);

        return Result.Ok();
    }

    /// <summary>
    /// Shares this document with an Group.
    /// </summary>
    /// <param name="groupId">The Group Id.</param>
    /// <returns>The result of the operation.</returns>
    public Result ShareWithGroup(GroupId groupId)
    {
        if (sharedGroupIds.Any(x => x.Equals(groupId)))
        {
            return Result.Fail($"This Document is already shared with this Group({groupId.Value})");
        }

        sharedGroupIds.Add(groupId);

        return Result.Ok();
    }

    /// <summary>
    /// Unshares this document with an User.
    /// </summary>
    /// <param name="userId">The User Id.</param>
    /// <returns>The result of the operation.</returns>
    public Result UnshareWithUser(UserId userId)
    {
        if (!sharedUserIds.Contains(userId))
        {
            return Result.Fail($"This Document is not shared with this User({userId.Value})");
        }

        sharedUserIds.Remove(userId);

        return Result.Ok();
    }

    /// <summary>
    /// Unshares this document with an Group.
    /// </summary>
    /// <param name="groupId">The Group Id.</param>
    /// <returns>The result of the operation.</returns>
    public Result UnshareWithGroup(GroupId groupId)
    {
        if (!sharedGroupIds.Contains(groupId))
        {
            return Result.Fail($"This Document is not shared with this Group({groupId.Value})");
        }

        sharedGroupIds.Remove(groupId);

        return Result.Ok();
    }

    /// <inheritdoc/>
    protected override IValidator GetValidator() => new DocumentValidator();
}
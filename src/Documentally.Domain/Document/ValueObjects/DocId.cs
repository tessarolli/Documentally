// <copyright file="DocId.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Domain.Common.DDD;

namespace Documentally.Domain.Doc.ValueObjects;

/// <summary>
/// Document Id Value Object.
/// </summary>
public sealed class DocId : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocId"/> class.
    /// </summary>
    /// <param name="id">Id value if exists.</param>
    public DocId(long? id = null)
    {
        if (id is null)
        {
            Value = 0;
        }
        else
        {
            Value = id.Value;
        }
    }

    /// <summary>
    /// Gets the Document ID.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Method required for comparing value objects.
    /// </summary>
    /// <returns>An ienumerable with all the properties of the value object.</returns>
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

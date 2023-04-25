// <copyright file="MimeTypeMappingService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

using Documentally.Application.Abstractions.Services;
using MimeTypes;

namespace Documentally.Infrastructure.Services;

/// <summary>
/// Mime Type Mapping Service.
/// </summary>
public class MimeTypeMappingService : IMimeTypeMappingService
{
    /// <inheritdoc/>
    public string GetMimeType(string fileName) => MimeTypeMap.GetMimeType(fileName);
}

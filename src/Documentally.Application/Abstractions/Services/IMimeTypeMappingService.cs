// <copyright file="IMimeTypeMappingService.cs" company="Documentally">
// Copyright (c) Documentally. All rights reserved.
// </copyright>

namespace Documentally.Application.Abstractions.Services;

/// <summary>
/// Mimi Type Mapping Service Interface.
/// </summary>
public interface IMimeTypeMappingService
{
    /// <summary>
    /// Gets the mime type from file name.
    /// </summary>
    /// <param name="fileName">file name.</param>
    /// <returns>mime type.</returns>
    string GetMimeType(string fileName);
}

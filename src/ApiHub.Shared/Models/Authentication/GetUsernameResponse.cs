// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace ApiHub.Shared.Models.Authentication;

public record GetUsernameResponse
{
    public required string Username { get; init; }
}

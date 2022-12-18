// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using ApiHub.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHub.Server.Controllers;

/// <summary>
/// Authentication related actions.
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Get the username of current user.
    /// </summary>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             Username of current user.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("username")]
    public IActionResult GetUsername()
    {
        if (User.Identity?.Name is null)
        {
            return Unauthorized();
        }
        
        return Ok(new GetUsernameResponse
        {
            Username = User.Identity.Name
        });
    }
    
    /// <summary>
    /// An empty endpoint which redirect to the OIDC login page.
    /// </summary>
    /// <param name="redirect">The URL redirect to after login.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>302</term>
    ///         <description>
    ///             Redirect to OIDC login page, after login, redirect to
    ///             the URL provided by the query string.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [Authorize]
    [HttpGet("login")]
    public RedirectResult Login([FromQuery] string redirect)
    {
        return Redirect(redirect);
    }

    /// <summary>
    /// Logout from Cookie and OIDC.
    /// </summary>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>302</term>
    ///         <description>
    ///             Redirect to the OIDC logout page.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [Authorize]
    [HttpGet("logout")]
    public SignOutResult Logout()
    {
        return SignOut(
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}

// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiHub.Server.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthenticationController : ControllerBase
{
    [Authorize]
    [HttpGet("username")]
    public IActionResult GetTest()
    {
        if (User.Identity?.Name is null)
        {
            return Unauthorized();
        }

        return Ok(User.Identity.Name);
    }
    
    [Authorize]
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string redirect)
    {
        return Redirect(redirect);
    }

    [Authorize]
    [HttpGet("logout")]
    public IActionResult Logout([FromQuery] string redirect)
    {
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = redirect
        }, 
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}

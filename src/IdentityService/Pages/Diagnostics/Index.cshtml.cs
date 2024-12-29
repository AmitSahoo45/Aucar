// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    public ViewModel View { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        // while running in docker, the remote address is the address of the container
        // so we need to check the local address as well
        // this might run locally but not in docker
        var localAddresses = new List<string?> { "127.0.0.1", "::1" };

        // the both if statements can also be commented out for removing these checks
        // if(HttpContext.Connection.LocalIpAddress != null)
        // {
        //     localAddresses.Add(HttpContext.Connection.LocalIpAddress.ToString());
        // }

        // if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
        // {
        //     return NotFound();
        // }

        View = new ViewModel(await HttpContext.AuthenticateAsync());
            
        return Page();
    }
}

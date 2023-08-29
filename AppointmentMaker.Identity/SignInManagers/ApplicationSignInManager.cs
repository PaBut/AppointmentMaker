﻿using AppointmentMaker.Identity.Entities.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Identity.SignInManagers;

internal class ApplicationSignInManager<TUser> : SignInManager<TUser> where TUser : IdentityUser
{
    public ApplicationSignInManager(UserManager<TUser> userManager, 
        IHttpContextAccessor contextAccessor, 
        IUserClaimsPrincipalFactory<TUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<TUser>> logger, 
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<TUser> confirmation)
        : base(userManager, contextAccessor, claimsFactory,
            optionsAccessor, logger, schemes, confirmation)
    {
    }
}

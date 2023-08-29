using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Identity.Entities.Users.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AppointmentMaker.Identity.Services.Base;

public abstract class UserService<TUSer> : IUserService
    where TUSer : AbstractUser
{
    protected readonly UserManager<TUSer> _userManager;
    private readonly HttpContext _httpContext; 

    public UserService(UserManager<TUSer> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<string?> GetUserId()
    {
        return Task.FromResult(_httpContext.User.FindFirstValue("uid"));
    }

    public async Task<bool> IsEmailAvailable(string email)
    {
        return (await _userManager.FindByEmailAsync(email)) == null;
    }

    public async Task<bool> UserExists(string Id)
    {
        return (await _userManager.FindByIdAsync(Id)) != null;
    }
}

using Application.Common.Interfaces;
using Domain.Helpers;

namespace Web.Services;

public sealed class CurrentUserManager(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
    : ICurrentUserService
{
    public Guid UserId => GetUserId();

    public string IpAddress => GetIpAddress();

    private Guid GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst("uid")?.Value;

        return userId is null ? Guid.Empty : Guid.Parse(userId);
    }

    private string GetIpAddress()
    {
        if (env.IsDevelopment())
            return IpHelper.GetIpAddress();

        if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            return httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
        else
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
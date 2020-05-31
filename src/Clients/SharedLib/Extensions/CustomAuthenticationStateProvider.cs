using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace SharedLib.Extensions
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        public void SignInUser(User user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Token", user.Token)
            }, "conduit");

            _claimsPrincipal = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(_claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public void SignOutUser()
        {
            _claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(_claimsPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public string GetToken()
        {
            Claim claim = _claimsPrincipal.FindFirst("Token");
            return claim?.Value;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_claimsPrincipal == null)
            {
                var identity = new ClaimsIdentity();
                _claimsPrincipal = new ClaimsPrincipal(identity);
            }
            return Task.FromResult(new AuthenticationState(_claimsPrincipal));
        }
    }
}

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Clients;
using Application.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace ServerClient.Extensions
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IConduitClient _conduitClient;
        private ClaimsPrincipal _claimsPrincipal;

        public CustomAuthenticationStateProvider(IConduitClient conduitClient)
        {
            _conduitClient = conduitClient ?? throw new ArgumentNullException(nameof(conduitClient));
        }

        public async Task<User> LoginAsync(Login login)
        {
            var response = await _conduitClient.LoginAsync(login);
            Authenticate(response);
            return response;
        }

        public async Task LoginWithTokenAsync(string token)
        {
            if (token == null) return;
            var response = await _conduitClient.LoginWithTokenAsync(token);
            Authenticate(response);
        }

        private void Authenticate(User user)
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

using BessPressPortal.Client.Models;
using BessPressPortal.Client.Services.BessPressPortal.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BessPressPortal.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly SecureStorage _secureStorage;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthStateProvider(HttpClient http, SecureStorage secureStorage)
        {
            _http = http;
            _secureStorage = secureStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _secureStorage.GetTokenAsync();
            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(_anonymous);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var identity = JwtParser.ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(identity, "jwt"));
            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication(string token)
        {
            var identity = JwtParser.ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(identity, "jwt"));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        // Optional: Add a method to save the token
        public async Task SetTokenAsync(string token)
        {
            await _secureStorage.SetTokenAsync(token);
            NotifyUserAuthentication(token);
        }

        // Optional: Add a method to clear the token
        public async Task ClearTokenAsync()
        {
            await _secureStorage.ClearTokenAsync();
            NotifyUserLogout();
        }
    }
}
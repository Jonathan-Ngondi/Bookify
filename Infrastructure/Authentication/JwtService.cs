using Application.Abstractions.Authentication;
using Domain.Abstractions;
using Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    internal sealed class JwtService : IJwtService
    {
        private static readonly Error AuthenticationFailed = new(
            "KeyCloak.AuthenticationFailed",
            "Failed to acquire access token due to authentication failure.");

        private readonly HttpClient _httpClient;
        private readonly KeyCloakOptions _keyCloakOptions;

        public JwtService(
            HttpClient httpClient,
            IOptions<KeyCloakOptions> keyCloakOptions)
        {
            _httpClient = httpClient;
            _keyCloakOptions = keyCloakOptions.Value;
        }

        public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            try
            {
                var authRequestParameters = new KeyValuePair<string, string>[]
                {
                    new("client_id", _keyCloakOptions.AuthClientId),
                    new("client_secret", _keyCloakOptions.AuthClientSecret),
                    new("scope", "openid email"),
                    new("grant_type", "password"),
                    new("username", email),
                    new("password", password)

                };

                var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

                var response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);

                response.EnsureSuccessStatusCode();

                var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>();

                if(authorizationToken is null)
                {
                    return Result.Failure<string>(AuthenticationFailed);
                }

                return authorizationToken.AccessToken;
            }
            catch (HttpRequestException)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }
        }
    }
}

﻿using Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public sealed class AdminAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly KeyCloakOptions _keyCloakOptions;

        public AdminAuthorizationDelegatingHandler(IOptions<KeyCloakOptions> keyCloakOptions)
        {
            _keyCloakOptions = keyCloakOptions.Value;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var authorizationToken = await GetAuthorizationToken(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                authorizationToken.AccessToken);

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }

        private async Task<AuthorizationToken> GetAuthorizationToken(CancellationToken cancellationToken)
        {
            var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keyCloakOptions.AdminClientId),
            new("client_secret", _keyCloakOptions.AdminClientSecret),
            new("scope", "openid email"),
            new("grant_type", "client_credentials")
        };

            var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);

            var authorizationRequest = new HttpRequestMessage(
                HttpMethod.Post,
                new Uri(_keyCloakOptions.TokenUrl))
            {
                Content = authorizationRequestContent
            };

            var authorizationResponse = await base.SendAsync(authorizationRequest, cancellationToken);

            authorizationResponse.EnsureSuccessStatusCode();

            return await authorizationResponse.Content.ReadFromJsonAsync<AuthorizationToken>() ??
                throw new ApplicationException("Failed to read authorization token from response.");
        }
    }
}

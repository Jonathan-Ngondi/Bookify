using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private const string PasswordCredentialType = "password";

        private readonly HttpClient _httpClient;
        
        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            var userRepresentationModel = UserRepresentationModel.FromUser(user);

            userRepresentationModel.Credentials =
            [
                new()
                {
                    Type = PasswordCredentialType,
                    Temporary = false,
                    Value = password
                }
            ];

            var response = await _httpClient.PostAsJsonAsync(
                "users",
                userRepresentationModel,
                cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            const string usersSegmentName = "users/";

            var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

            if(locationHeader is null)
            {
                throw new InvalidOperationException("Location header is missing in the response.");
            }

            var userSegmentValueIndex = locationHeader.IndexOf(
                usersSegmentName, 
                StringComparison.InvariantCultureIgnoreCase);

            var userIdentityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

            return userIdentityId;
        }
    }
}

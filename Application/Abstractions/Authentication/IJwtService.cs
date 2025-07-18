using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Authentication
{
    public interface IJwtService
    {
        Task<Result<string>> GetAccessTokenAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default);
    }
}

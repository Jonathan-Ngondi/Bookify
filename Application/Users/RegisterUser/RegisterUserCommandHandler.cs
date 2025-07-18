﻿using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.RegisterUser
{
    internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IAuthenticationService _authencitionService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IAuthenticationService authencitionService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _authencitionService = authencitionService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                new FirstName(request.FirstName),
                new LastName(request.LastName),
                new Email(request.Email));

            var identityId = await _authencitionService.RegisterAsync(
                user,
                request.Password,
                cancellationToken);

            user.SetIdentityId(identityId);
            
            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}

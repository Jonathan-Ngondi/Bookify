using System;
using Domain.Abstractions;
using Domain.Users.Events;

namespace Domain.Users;

public sealed class User : Entity
{
	private readonly List<Role> _roles = new();
	public User(Guid id,
		FirstName firstName,
		LastName lastName,
		Email email) : base(id)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
	}

	private User()
	{
        // Required for EF Core
    }

    public FirstName FirstName { get; private set; }
	public LastName LastName { get; private set; }
	public Email Email { get; private set; }

    public string IdentityId { get; private set; } = string.Empty;

	public IReadOnlyList<Role> Roles => _roles.ToList();

    public static User Create(FirstName firstName, LastName lastName, Email email)
	{
		var user = new User(Guid.NewGuid(), firstName, lastName, email);

		user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

		user._roles.Add(Role.Registered);

        return user;
	}

    public void SetIdentityId(string identityId)
    {
		IdentityId = identityId;
    }

	
}


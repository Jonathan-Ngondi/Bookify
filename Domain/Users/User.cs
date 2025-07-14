using System;
using Domain.Abstractions;
using Domain.Users.Events;

namespace Domain.Users;

public sealed class User : Entity
{
	public User(Guid id,
		FirstName firstName,
		LastName lastName,
		Email email) : base(id)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
	}

	public FirstName FirstName { get; private set; }
	public LastName LastName { get; private set; }
	public Email Email { get; private set; }

	public static User Create(FirstName firstName, LastName lastName, Email email)
	{
		var user = new User(Guid.NewGuid(), firstName, lastName, email);

		user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

		return user;
	}
}


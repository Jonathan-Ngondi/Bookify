﻿using System;
using Application.Exceptions;
using Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
	public ApplicationDbContext(DbContextOptions options, IPublisher publisher)
		: base(options)
	{
        _publisher = publisher;
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var trackedEntries = ChangeTracker.Entries().Count();
            var result = await base.SaveChangesAsync(cancellationToken);

            var done = await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }

    }

    private async Task<string> PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
        return "Done";
    }
}


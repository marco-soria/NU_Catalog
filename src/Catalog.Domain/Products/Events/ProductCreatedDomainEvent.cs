using Catalog.Domain.Abstractions;

namespace Catalog.Domain.Products.Events;

public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
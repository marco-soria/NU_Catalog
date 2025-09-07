using NU_Catalog.Domain.Abstractions;

namespace NU_Catalog.Domain.Products.Events;

public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
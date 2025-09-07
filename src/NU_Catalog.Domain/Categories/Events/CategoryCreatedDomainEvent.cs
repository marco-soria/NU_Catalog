using NU_Catalog.Domain.Abstractions;

namespace NU_Catalog.Domain.Categories.Events;

public sealed record CategoryCreatedDomainEvent(Guid categoryId): IDomainEvent;
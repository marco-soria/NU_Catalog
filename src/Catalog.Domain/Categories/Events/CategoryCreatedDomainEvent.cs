using Catalog.Domain.Abstractions;

namespace Catalog.Domain.Categories.Events;

public sealed record CategoryCreatedDomainEvent(Guid categoryId): IDomainEvent;
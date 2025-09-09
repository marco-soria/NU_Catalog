using Catalog.Domain.Abstractions;
using Catalog.Domain.Categories.Events;


namespace Catalog.Domain.Categories;

public class Category : Entity
{
   
    public string Name {get; private set;} = default!;
    private Category(){}
    private Category(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Category Create(string name)
    {
        var category = new Category(Guid.NewGuid(), name);
        var categoryDomainEvent = new CategoryCreatedDomainEvent(category.Id);
        category.RaiseDomainEvent(categoryDomainEvent);
        return category;
    }
}